namespace Dota.Auth.Services
{
    public interface IEmailService
    {
        void SendEmail(string email, string message);
    }
}