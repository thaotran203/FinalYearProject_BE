namespace FinalYearProject_BE.DTOs
{
    public class PaymentRequestDTO
    {
        public int UserId {  get; set; }
        public int CourseId { get; set; }
        public double Amount { get; set; }
    }
}
