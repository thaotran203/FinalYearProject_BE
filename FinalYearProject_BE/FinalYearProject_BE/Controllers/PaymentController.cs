using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _config;

        public PaymentController(IPaymentService paymentService, IConfiguration config)
        {
            _paymentService = paymentService;
            _config = config;
        }

        [HttpPost("CreatePaymentUrl")]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentRequestDTO paymentRequestDto)
        {
            if (paymentRequestDto == null || paymentRequestDto.Amount <= 0 || paymentRequestDto.CourseId <= 0 || paymentRequestDto.UserId <= 0)
            {
                return BadRequest("Invalid payment request data.");
            }

            // Generate the payment URL
            var paymentUrl = await _paymentService.CreatePaymentUrl(HttpContext, paymentRequestDto);

            return Ok(new { PaymentUrl = paymentUrl });
        }


        [HttpGet("PaymentCallback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var queryParameters = Request.Query;

            if (!queryParameters.Any())
            {
                return BadRequest("No query parameters found in the callback.");
            }

            try
            {
                var paymentResponse = await _paymentService.PaymentExecute(queryParameters);

                if (paymentResponse.Success)
                {
                    return Redirect("https://edmicroielts.netlify.app/");
                }
                else
                {
                    return BadRequest("Payment verification failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the payment callback: {ex.Message}");
            }
        }

        [HttpGet("PaymentHistories")]
        public async Task<IActionResult> GetAllPaymentHistories()
        {
            var history = await _paymentService.GetAllPaymentHistories();
            return Ok(history);
        }

        [HttpGet("PaymentHistoryOfStudent")]
        public async Task<IActionResult> GetPaymentHistoryForStudent()
        {
            var userId = int.Parse(User.FindFirst("Id")?.Value);
            var history = await _paymentService.GetPaymentHistoryForStudent(userId);
            return Ok(history);
        }

        [HttpGet("GetEnrolledCourses")]
        public async Task<IActionResult> GetEnrolledCourses()
        {
            var userId = int.Parse(User.FindFirst("Id")?.Value);
            var courses = await _paymentService.GetEnrolledCourses(userId);
            return Ok(courses);
        }
    }
}
