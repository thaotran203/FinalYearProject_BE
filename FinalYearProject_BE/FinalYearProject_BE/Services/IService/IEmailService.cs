namespace FinalYearProject_BE.Services.IService
{
    public interface IEmailService
    {
        Task SendPasswordResetEmail(string email, string resetLink);
        Task SendRegistrationEmail(string email, string password);
    }

}
