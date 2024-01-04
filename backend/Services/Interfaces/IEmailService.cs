namespace MiodOdStaniula.Services.Interfaces{
    public interface IEmailService
    {
        Task SendActivationEmail(string email, string userId, string name, string token);
    }
}