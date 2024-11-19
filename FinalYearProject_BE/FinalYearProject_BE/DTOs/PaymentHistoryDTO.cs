namespace FinalYearProject_BE.DTOs
{
    public class PaymentHistoryDTO
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public string CourseTitle { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
