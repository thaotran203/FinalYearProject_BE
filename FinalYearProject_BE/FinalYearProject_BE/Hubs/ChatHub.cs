using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using System.Text;
using System.Text.Json;
using System.Linq;
using Microsoft.Extensions.Logging;
using static FinalYearProject_BE.DTOs.AIResponseDTO;

namespace FinalYearProject_BE.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly HttpClient _aiServiceClient;
        private readonly HttpClient _ollamaClient;
        private readonly QdrantClient _qdrantClient;

        private const string QdrantCollectionName = "video_segments";

        public ChatHub(
            ILogger<ChatHub> logger,
            IHttpClientFactory factory,
            IConfiguration config)
        {
            _logger = logger;
            _aiServiceClient = factory.CreateClient();
            _aiServiceClient.BaseAddress = new Uri(config["ServiceUrls:AIService"]!);

            _ollamaClient = factory.CreateClient();
            _ollamaClient.BaseAddress = new Uri("http://localhost:11434");

            string qdrantHost = "localhost";
            int qdrantPort = 6334;
            _qdrantClient = new QdrantClient(qdrantHost, qdrantPort);
        }

        public async Task SendMessage(int videoId, string question)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(question))
                {
                    await Clients.Caller.SendAsync("ReceiveMessageToken", "Please enter a question.");
                    return;
                }

                _logger.LogInformation("RAG process started for VideoId: {VideoId}, Question: {Question}", videoId, question);

                // --- BƯỚC 1: EMBEDDING (RETRIEVE) ---
                var embedRequest = new { text = question };
                var embedResponse = await _aiServiceClient.PostAsJsonAsync("/embed", embedRequest);

                if (!embedResponse.IsSuccessStatusCode)
                {
                    var errorContent = await embedResponse.Content.ReadAsStringAsync();
                    throw new Exception($"Embedding Service Failed: {embedResponse.StatusCode} - {errorContent}");
                }

                var embedding = await embedResponse.Content.ReadFromJsonAsync<EmbeddingResponse>();
                if (embedding == null || embedding.Vector == null)
                    throw new Exception("Embedding response was null or invalid.");

                // --- BƯỚC 2: SEARCH QDRANT ---
                float scoreThreshold = 0.3f;

                var searchResult = await _qdrantClient.SearchAsync(
                    collectionName: QdrantCollectionName,
                    vector: embedding.Vector.ToArray(),
                    limit: 3,
                    scoreThreshold: scoreThreshold,
                    filter: new Filter
                    {
                        Must =
                        {
                            new Condition
                            {
                                Field = new FieldCondition
                                {
                                    Key = "videoId",
                                    Match = new Match { Integer = (long)videoId }
                                }
                            }
                        }
                    }
                );

                // --- BƯỚC 3: BUILD PROMPT (AUGMENT) ---
                var promptBuilder = new StringBuilder();

                promptBuilder.AppendLine("### SYSTEM INSTRUCTIONS ###");
                promptBuilder.AppendLine("You are a smart AI Tutor for a video lesson. Your goal is to answer based on the provided TRANSCRIPT.");

                promptBuilder.AppendLine("\n--- PRIORITY RULES ---");
                // Rule 1
                promptBuilder.AppendLine("1. **GREETINGS:** If the input is 'hello', 'hi', 'how are you', etc., reply politely: 'Hello! Do you have a question about the video?'");

                // Rule 2
                promptBuilder.AppendLine("2. **GIBBERISH DETECTOR:** If the input is random characters (e.g., 'asdf', 'kakaka', '1234') or just one or two letters, reply: 'I'm sorry, I didn't understand. Please ask a specific question.'");

                // Rule 3
                promptBuilder.AppendLine("3. **OUT OF SCOPE (Social/General):** If the user asks personal questions about YOU (e.g., 'Where are you from', 'Are you a robot') or general knowledge not in the video, reply: 'I am an AI assistant focusing on this video lesson only.'");

                // Rule 4
                promptBuilder.AppendLine("4. **CONTENT QUESTIONS:**");
                promptBuilder.AppendLine("   - Check the [VIDEO TRANSCRIPT SEGMENTS] below.");
                promptBuilder.AppendLine("   - If the answer is found: Explain it clearly.");
                promptBuilder.AppendLine("   - If the transcript is EMPTY or the answer is NOT there: Reply strictly: 'Sorry, I couldn't find information about that in this video.'");

                promptBuilder.AppendLine("\n### VIDEO TRANSCRIPT SEGMENTS ###");

                if (searchResult.Count == 0)
                {
                    promptBuilder.AppendLine("[(NO DATA) No relevant video segments found for this question.]");
                }
                else
                {
                    foreach (var point in searchResult)
                    {
                        if (point.Payload.TryGetValue("text", out var textValue))
                        {
                            promptBuilder.AppendLine($"- {textValue.StringValue}");
                        }
                    }
                }

                promptBuilder.AppendLine("### END OF TRANSCRIPT ###");
                promptBuilder.AppendLine($"\nStudent's Question: {question}");
                promptBuilder.AppendLine("\nAI Answer:");

                var fullPrompt = promptBuilder.ToString();

                // --- BƯỚC 4: GENERATE (OLLAMA) ---

                var ollamaRequest = new
                {
                    model = "llama3:8b",
                    prompt = fullPrompt,
                    stream = true,
                    options = new
                    {
                        temperature = 0.1,
                        num_ctx = 4096
                    }
                };

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/generate")
                {
                    Content = new StringContent(JsonSerializer.Serialize(ollamaRequest), Encoding.UTF8, "application/json")
                };

                using var response = await _ollamaClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    var errorOllama = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Ollama Service Failed: {response.StatusCode} - {errorOllama}");
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    try
                    {
                        var ollamaPart = JsonSerializer.Deserialize<OllamaStreamResponse>(line);
                        if (ollamaPart?.response != null)
                        {
                            await Clients.Caller.SendAsync("ReceiveMessageToken", ollamaPart.response);
                        }
                    }
                    catch (JsonException)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ChatHub processing VideoId {VideoId}", videoId);
                await Clients.Caller.SendAsync("ReceiveMessageToken", "⚠️ System Error: I'm having trouble processing your request right now. Please try again later.");
            }
        }

        private record EmbeddingResponse(List<float> Vector);
        private record OllamaStreamResponse(string response, bool done);
    }
}