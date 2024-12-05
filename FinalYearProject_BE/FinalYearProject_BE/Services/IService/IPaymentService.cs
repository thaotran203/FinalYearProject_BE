using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentUrl(HttpContext context, PaymentRequestDTO paymentRequestDto, int userId);
        Task<PaymentResponseDTO> PaymentExecute(IQueryCollection collections);
        Task<List<PaymentHistoryDTO>> GetAllPaymentHistories();
        Task<List<PaymentHistoryDTO>> GetPaymentHistoryForStudent(int userId);
        Task<List<EnrolledCourseDTO>> GetEnrolledCourses(int userId);
    }
}
