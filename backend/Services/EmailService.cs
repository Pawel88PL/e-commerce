using System.Net;
using System.Net.Mail;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services{
    public class EmailService: IEmailService
    {
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
                                
            var mailMessage = new MailMessage()
            {
                From = new MailAddress("kontakt@miododstaniula.pl"),
                Subject = "Aktywacja Konta",
                Body = emailBody,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);
            
            var smtpClient = new SmtpClient("smtp.webio.pl")
            {
                Port = 587,
                Credentials = new NetworkCredential("kontakt@miododstaniula.pl", "Pasieka@21"),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}