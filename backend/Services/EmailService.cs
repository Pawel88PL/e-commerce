using System.Net;
using System.Net.Mail;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        private ILogger<EmailService> _logger;
        private readonly string? _baseUrl;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _baseUrl = _configuration["ApplicationSettings:BaseUrl"];
        }

        public async Task SendActivationEmail(string email, string userId, string name, string token)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            var activationLink = $"{_baseUrl}/activate?userId={userId}&token={encodedToken}";

            string emailBody = $@"
            
            <h1>Witaj {name}!</h1>
            <p>Twoje konto zostało utworzone.</p>
            <p>Aby aktywować swoje konto kliknij na link: <a href='{activationLink}'>Aktywuj Konto</a>
            <br>
            <p>W razie problemów z aktywacją konta napisz do nas na adres kontakt@miododstaniula.pl</p>
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

        public async Task SendOrderConfirmationEmail(string email, string name, OrderDTO order)
        {
            string emailBody = $@"
            <h1>Dziękujemy za złożenie zamówienia, {name}!</h1>
            <p>Twoje zamówienie numer {order.ShortOrderId} zostało przyjęte do realizacji.</p>
            <p>Oto szczegóły Twojego zamówienia:</p>
            <ul>";

            foreach (var detail in order.OrderDetails)
            {
                emailBody += $"<li>{detail.ProductName} - {detail.Quantity} sztuk(a) - {detail.UnitPrice * detail.Quantity:C}</li>";
            }

            emailBody += $@"
            </ul>
            <p>Łączny koszt: {order.TotalPrice:C}</p>
            <h2>Informacje o płatności</h2>
            <p>Abyśmy mogli przystąpić do realizacji Twojego zamówienia, prosimy o dokonanie płatności w jednej z poniższych form:</p>
            <ul>
                <li><strong>Przelew bankowy</strong>: Prosimy o dokonanie przelewu na numer konta: 37 1050 1171 1000 0091 1468 3544. W tytule przelewu prosimy o podanie numeru zamówienia: {order.ShortOrderId}.</li>
                <li><strong>BLIK</strong>: Możesz również dokonać płatności za pomocą BLIK pod numerem telefonu: 570 436 579. Jako tytuł płatności prosimy również o podanie numeru zamówienia: {order.ShortOrderId}.</li>
            </ul>
            <p>Twoje zamówienie zostanie przekazane do realizacji niezwłocznie po zaksięgowaniu wpłaty na naszym koncie.</p>
            <p>W razie pytań lub wątpliwości prosimy o kontakt pod numerem telefonu: +48 570 436 579 lub e-mail: kontakt@miododstaniula.pl.</p>
            <hr>
            <p>Pozdrawiamy,</p>
            <p>Rodzina Staniul</p>";

            await SendEmail(email, "Potwierdzenie zamówienia", emailBody);
        }

        public async Task SendNewOrderNotificationToOwner(OrderDTO order, UserModel user)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            string ownerEmail = emailSettings["OwnerEmail"] ?? " ";
            string subject = "Nowe zamówienie w sklepie MiodOdStaniula";

            string emailBody = $@"
            <h1>Masz nowe zamówienie!</h1>
            <p>Zamówienie od {user.Name} {user.Surname}</p>
            <ul>";

            foreach (var detail in order.OrderDetails)
            {
                emailBody += $"<li>{detail.ProductName} - {detail.Quantity} sztuk(a) - {detail.UnitPrice * detail.Quantity:C}</li>";
            }

            emailBody += $@"</ul>
            <p>Łączny koszt: {order.TotalPrice:C}</p>
            <p>Informacje kontaktowe klienta:</p>
            <p></p>
            <hr>
            <p>Prosimy o sprawdzenie panelu administracyjnego sklepu w celu dalszej realizacji zamówienia.</p>";

            await SendEmail(ownerEmail, subject, emailBody);
        }

        private async Task SendEmail(string to, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = Convert.ToInt32(emailSettings["SmtpPort"]);
            var smtpUsername = emailSettings["SmtpUsername"];
            var smtpPassword = emailSettings["SmtpPassword"];

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(smtpUsername ?? string.Empty),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

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
                _logger.LogError(ex, $"Wystąpił błąd przy wysyłaniu emaila: {subject}.");
            }
        }
    }
}