using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;
using NuGet.Protocol.Core.Types;

namespace FinalYearProject_BE.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task ProcessPayment(PaymentRequestDTO paymentRequest, int userId)
        {
            var payment = new PaymentModel
            {
                PaymentDate = DateTime.UtcNow,
                Amount = paymentRequest.Amount,
                UserId = userId,
                CourseId = paymentRequest.CourseId
            };

            await _paymentRepository.SavePayment(payment);

            var enrollment = new EnrollmentModel
            {
                EnrollmentDate = DateTime.UtcNow,
                UserId = userId,
                CourseId = paymentRequest.CourseId
            };

            await _paymentRepository.SaveEnrollment(enrollment);
        }

        public async Task<List<PaymentHistoryDTO>> GetAllPaymentHistories()
        {
            return await _paymentRepository.GetAllPaymentHistories();
        }

        public async Task<List<PaymentHistoryDTO>> GetPaymentHistoryForStudent(int userId)
        {
            return await _paymentRepository.GetPaymentHistoryForStudent(userId);
        }

        public async Task<List<EnrolledCourseDTO>> GetEnrolledCourses(int userId)
        {
            return await _paymentRepository.GetEnrolledCourses(userId);
        }

    }
}
