using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;

namespace FinalYearProject_BE.Repository.IRepository
{
    public interface IPaymentRepository
    {
        Task SavePayment(PaymentModel payment);
        Task SaveEnrollment(EnrollmentModel enrollment);
        Task<List<PaymentHistoryDTO>> GetAllPaymentHistories();
        Task<List<PaymentHistoryDTO>> GetPaymentHistoryForStudent(int userId);
        Task<List<EnrolledCourseDTO>> GetEnrolledCourses(int userId);
    }
}
