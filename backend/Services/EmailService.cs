using System.Globalization;
using System.Net;
using System.Net.Mail;
using backend.Interfaces;
using backend.Models;

namespace backend.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly string? _baseUrl;
        private decimal _shippingCost;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _baseUrl = _configuration["ApplicationSettings:BaseUrl"];
            _shippingCost = decimal.Parse(_configuration["ApplicationSettings:ShippingCost"]!, CultureInfo.InvariantCulture);
        }

        public async Task SendActivationEmail(string email, string userId, string name, string token)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            var activationLink = $"{_baseUrl}/account/activate?userId={userId}&token={encodedToken}";

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

            var deliveryMethod = order.IsPickupInStore ? "Odbiór osobisty" : "Kurier";

            if (order.IsPickupInStore)
            {
                _shippingCost = 0;
            }

            string emailBody = $@"
            <div style=""max-width: 800px;"">
            <h1>{name} dziękujemy za zakupy z naszej pasieki!</h1>
            <h2>Jesteśmy wdzięczni za zaufanie i wspieranie lokalnego, rodzinnego biznesu.</h2>
            <p>Nr twojego zamówienia: <strong> {order.ShortOrderId} </strong></p>
            <p>Wybrany sposób dostawy: <strong> {deliveryMethod} </strong></p>
            <h2>Szczegóły zamówionych produktów</h2>
            <table style=""width: 100%; border-collapse: collapse;"">
                <thead>
                    <tr>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Nazwa produktu</th>
                        <th style=""text-align: right; border: 1px solid #ddd; padding: 8px;"">Ilość</th>
                        <th style=""text-align: right; border: 1px solid #ddd; padding: 8px;"">Cena jednostkowa</th>
                        <th style=""text-align: right; border: 1px solid #ddd; padding: 8px;"">Suma</th>
                    </tr>
                </thead>
                <tbody>";

            foreach (var detail in order.OrderDetails)
            {
                emailBody += $@"
                <tr>
                    <td style=""border: 1px solid #ddd; padding: 8px;"">{detail.ProductName}</td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: right;"">{detail.Quantity}</td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: right;"">{detail.UnitPrice:C}</td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: right;"">{detail.UnitPrice * detail.Quantity:C}</td>
                </tr>";
            }

            emailBody += $@"
                <tr>
                    <td style=""border: 1px solid #ddd; padding: 8px;"" colspan=""3"">Koszt dostawy</td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: right;"">{_shippingCost:C}</td>
                </tr>
                <tr>
                    <td style=""border: 1px solid #ddd; padding: 8px;"" colspan=""3""><strong>Łącznie</strong></td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: right;""><strong>{order.TotalPrice:C}</strong></td>
                </tr>
            </tbody>
            </table>
            <br>
            <p>W razie pytań lub wątpliwości prosimy o kontakt pod numerem telefonu: </p>
            <p><strong>+48 570 436 579</strong></p>
            <p>lub e-mail:</p>
            <p>kontakt@miododstaniula.pl</p>
            <hr>
            <p>Pozdrawiamy,</p>
            <p>Rodzina Staniul</p>
            </div>";

            await SendEmail(email, "Potwierdzenie zamówienia", emailBody);
        }

        public async Task SendNewOrderNotificationToOwner(OrderDTO order, UserModel user)
        {

            var deliveryMethod = order.IsPickupInStore ? "Odbiór osobisty" : "Kurier";

            var emailSettings = _configuration.GetSection("EmailSettings");
            string ownerEmail = emailSettings["OwnerEmail"] ?? " ";
            string subject = "Nowe zamówienie w sklepie Miód Od Staniula";

            string emailBody = $@"
            <div style=""max-width: 800px;"">
            <h1>Masz nowe zamówienie!</h1>
            <h3>Numer zamówienia:</h3>
            <p> {order.OrderId} </p>
            <h2>Szczegóły zamówionych produktów</h2>
            <table style=""width: 100%; border-collapse: collapse;"">
                <thead>
                    <tr>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Nazwa produktu</th>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Ilość</th>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Cena jednostkowa</th>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Całkowita cena</th>
                    </tr>
                </thead>
                <tbody>";

            foreach (var detail in order.OrderDetails)
            {
                emailBody += $@"
                <tr>
                    <td style=""border: 1px solid #ddd; padding: 8px;"">{detail.ProductName}</td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: right;"">{detail.Quantity}</td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: right;"">{detail.UnitPrice:C}</td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: right;"">{detail.UnitPrice * detail.Quantity:C}</td>
                </tr>";
            }

            emailBody += $@"
                </tbody>
            </table>
            <br>
            <p><strong>Łączna kwota zamówienia: {order.TotalPrice:C} </strong></p>
            <p><strong>Metoda dostawy: {deliveryMethod} </strong></p>
            <hr>
            <h2>Klient:</h2>
            <table style=""width: 100%; max-width: 500px; border-collapse: collapse;"">
                <tbody>
                    <tr>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Imię i Nazwisko</th>
                        <td style=""border: 1px solid #ddd; padding: 8px;""> {user.Name} {user.Surname} </td>
                    </tr>
                    <tr>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Miasto</th>
                        <td style=""border: 1px solid #ddd; padding: 8px;""> {user.City} </td>
                    </tr>
                    <tr>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Ulica</th>
                        <td style=""border: 1px solid #ddd; padding: 8px;""> {user.Street} </td>
                    </tr>
                    <tr>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Numer domu</th>
                        <td style=""border: 1px solid #ddd; padding: 8px;""> {user.Address} </td>
                    </tr>
                    <tr>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Telefon</th>
                        <td style=""border: 1px solid #ddd; padding: 8px;""> {user.PhoneNumber} </td>
                    </tr>
                    <tr>
                        <th style=""text-align: left; border: 1px solid #ddd; padding: 8px;"">Email</th>
                        <td style=""border: 1px solid #ddd; padding: 8px;""> {user.Email} </td>
                    </tr>
                </tbody>
            </table>
            <hr>
            <p>Zaloguj się do panelu administracyjnego sklepu w celu dalszej realizacji zamówienia.</p>
            </div>";

            await SendEmail(ownerEmail, subject, emailBody);
        }

        public async Task SendOrderStatusChangeEmail(string email, string name, OrderDTO order)
        {
            string emailBody = $@"
            <h1>Witaj {name}!</h1>
            <p>Status Twojego zamówienia o numerze: <strong>{order.ShortOrderId}</strong> został zmieniony na: <strong>{order.Status}</strong></p>
            <p>W razie pytań lub wątpliwości prosimy o kontakt pod numerem telefonu: </p>
            <p><strong>+48 570 436 579</strong></p>
            <p>lub e-mail: kontakt@miododstaniula.pl</p>
            <hr>
            <p>Pozdrawiamy,</p>
            <p>Rodzina Staniul</p>";
            
            await SendEmail(email, "Zmiana statusu zamówienia", emailBody);
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