using FinalYearProject_BE.Data;
using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject_BE.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SavePayment(PaymentModel payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task SaveEnrollment(EnrollmentModel enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PaymentHistoryDTO>> GetAllPaymentHistories()
        {
            var paymentHistories = await (from payment in _context.Payments
                                          join user in _context.Users on payment.UserId equals user.Id
                                          join course in _context.Courses on payment.CourseId equals course.Id
                                          select new PaymentHistoryDTO
                                          {
                                              Id = payment.Id,
                                              PaymentDate = payment.PaymentDate,
                                              Amount = course.Price,
                                              CourseTitle = course.Title,
                                              UserId = user.Id,
                                              UserName = user.FullName
                                          }).ToListAsync();

            return paymentHistories;
        }

        public async Task<List<PaymentHistoryDTO>> GetPaymentHistoryForStudent(int userId)
        {
            var paymentHistories = await (from payment in _context.Payments
                                          join user in _context.Users on payment.UserId equals user.Id
                                          join course in _context.Courses on payment.CourseId equals course.Id
                                          where payment.UserId == userId
                                          select new PaymentHistoryDTO
                                          {
                                              Id = payment.Id,
                                              PaymentDate = payment.PaymentDate,
                                              Amount = course.Price,
                                              CourseTitle = course.Title,
                                          }).ToListAsync();

            return paymentHistories;
        }

        public async Task<List<EnrolledCourseDTO>> GetEnrolledCourses(int userId)
        {
            var result = await (from enrollment in _context.Enrollments
                                join course in _context.Courses on enrollment.CourseId equals course.Id
                                join user in _context.Users on course.InstructorId equals user.Id
                                where enrollment.UserId == userId
                                select new EnrolledCourseDTO
                                {
                                    CourseId = course.Id,
                                    CourseTitle = course.Title,
                                    ImageLink = course.ImageLink,
                                    EnrollmentDate = enrollment.EnrollmentDate,
                                    InstructorId = course.InstructorId,
                                    TeacherName = user.FullName
                                }).ToListAsync();

            return result;
        }
    }
}
