namespace FinalYearProject_BE.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public int RoleId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
