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

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDTO paymentRequest)
        {
            var userId = int.Parse(User.FindFirst("Id")?.Value);
            await _paymentService.ProcessPayment(paymentRequest, userId);
            return Ok("Payment processed and enrollment created.");
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
