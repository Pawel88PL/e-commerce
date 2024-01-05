using System.Net;
using System.Net.Mail;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        private ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendActivationEmail(string email, string userId, string name, string token)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            var activationLink = $"https://localhost:5047/activate?userId={userId}&token={encodedToken}";

            string emailBody = $@"
            
            <h1>Witaj {name}!</h1>
            <p>Twoje konto zostało utworzone.</p>
            <p>Aby aktywować swoje konto kliknij proszę na link obok: <a href='{activationLink}'>Aktywuj Konto</a>
            <br>
            <p>W razie problemów z aktywacją konta prosimy o napisz do nas na adres kontakt@miododstaniula.pl</p>
            <p>lub zadzwoń pod numer telefonu: 570 436 579</p>
            <br>
            <hr>
            <p>Pozdrawiamy,</p>
            <p>Rodzina Staniul</p>";

            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = Convert.ToInt32(emailSettings["SmtpPort"]);
            var smtpUsername = emailSettings["SmtpUsername"];
            var smtpPassword = emailSettings["SmtpPassword"];

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(smtpUsername ?? string.Empty),
                Subject = "Aktywacja Konta",
                Body = emailBody,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd przy wysyłaniu emaila aktywacyjnego.");
            }
        }
    }
}