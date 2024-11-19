using FinalYearProject_BE.DTOs;

namespace FinalYearProject_BE.Services.IService
{
    public interface IPaymentService
    {
        Task ProcessPayment(PaymentRequestDTO paymentRequest, int userId);
        Task<List<PaymentHistoryDTO>> GetAllPaymentHistories();
        Task<List<PaymentHistoryDTO>> GetPaymentHistoryForStudent(int userId);
        Task<List<EnrolledCourseDTO>> GetEnrolledCourses(int userId);
    }
}
