using FinalYearProject_BE.DTOs;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Services.IService;
using FinalYearProject_BE.Settings;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace FinalYearProject_BE.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IConfiguration _config;

        public PaymentService(IPaymentRepository paymentRepository, IConfiguration config)
        {
            _paymentRepository = paymentRepository;
            _config = config;
        }


        public Task<string> CreatePaymentUrl(HttpContext context, PaymentRequestDTO paymentRequestDto)
        {
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
            var tick = DateTime.Now.Ticks.ToString();
            var vnPay = new VnPayLibrary();

            var orderInfo = $"{paymentRequestDto.UserId}|{paymentRequestDto.CourseId}";

            vnPay.AddRequestData("vnp_Version", _config["Vnpay:Version"]);
            vnPay.AddRequestData("vnp_Command", _config["Vnpay:Command"]);
            vnPay.AddRequestData("vnp_TmnCode", _config["Vnpay:TmnCode"]);
            vnPay.AddRequestData("vnp_Amount", (paymentRequestDto.Amount * 100).ToString());
            vnPay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", _config["Vnpay:CurrCode"]);
            vnPay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnPay.AddRequestData("vnp_Locale", _config["Vnpay:Locale"]);
            vnPay.AddRequestData("vnp_OrderInfo", orderInfo);
            vnPay.AddRequestData("vnp_OrderType", "other");
            vnPay.AddRequestData("vnp_ReturnUrl", _config["Vnpay:ReturnUrl"]);
            vnPay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = vnPay.CreateRequestUrl(_config["Vnpay:BaseUrl"], _config["Vnpay:HashSecret"]);

            return Task.FromResult(paymentUrl);
        }

        public async Task<PaymentResponseDTO> PaymentExecute(IQueryCollection collections)
        {
            var vnPay = new VnPayLibrary();

            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(key, value.ToString());
                }
            }

            var orderId = Convert.ToInt64(vnPay.GetResponseData("vnp_TxnRef"));
            var vnPayTranId = Convert.ToInt64(vnPay.GetResponseData("vnp_TransactionNo"));
            var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
            var vnpSecureHash = collections.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value;
            var orderInfo = vnPay.GetResponseData("vnp_OrderInfo");

            // Validate signature
            bool checkSignature = vnPay.ValidateSignature(vnpSecureHash, _config["Vnpay:HashSecret"]);

            if (!checkSignature)
            {
                return new PaymentResponseDTO
                {
                    Success = false,
                    Message = "Invalid signature."
                };
            }

            if (vnpResponseCode != "00")
            {
                return new PaymentResponseDTO
                {
                    Success = false,
                    Message = "Payment not successful."
                };
            }

            var orderInfoParts = orderInfo.Split('|');
            if (orderInfoParts.Length != 2)
                throw new Exception("Invalid order information format.");

            var userId = int.Parse(orderInfoParts[0]);
            var courseId = int.Parse(orderInfoParts[1]);

            // Save payment
            var payment = new PaymentModel
            {
                PaymentDate = DateTime.UtcNow,
                Amount = Convert.ToDouble(vnPay.GetResponseData("vnp_Amount")) / 100,
                UserId = userId,
                CourseId = courseId
            };

            await _paymentRepository.SavePayment(payment);

            // Save enrollment
            var enrollment = new EnrollmentModel
            {
                EnrollmentDate = DateTime.UtcNow,
                UserId = userId,
                CourseId = courseId
            };

            await _paymentRepository.SaveEnrollment(enrollment);

            return new PaymentResponseDTO
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = orderInfo,
                OrderId = orderId.ToString(),
                PaymentId = vnPayTranId.ToString(),
                TransactionId = vnPayTranId.ToString(),
                Token = vnpSecureHash,
                VnPayResponseCode = vnpResponseCode
            };
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
