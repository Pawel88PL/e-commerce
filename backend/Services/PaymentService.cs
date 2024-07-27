using System.Security.Cryptography;
using System.Text;
using backend.Data;
using backend.Interfaces;
using backend.Models;

namespace backend.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentService(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        private string GeneratePaymentFormHtml(ServiceRequest request)
        {
            var destination = _configuration["PaymentService:eCommerce"]
                ?? throw new Exception("Destination is not configured.");

            var formHtml = $@"
            <html>
            <body onload='document.forms[0].submit()'>
                <form action='{destination}' method='post'>
                    <input type='hidden' name='pos_id' value='{request.Pos_id}'>
                    <input type='hidden' name='order_id' value='{request.Order_id}'>
                    <input type='hidden' name='session_id' value='{request.Session_id}'>
                    <input type='hidden' name='amount' value='{request.Amount}'>
                    <input type='hidden' name='currency' value='{request.Currency}'>
                    <input type='hidden' name='test' value='{request.Test}'>
                    <input type='hidden' name='language' value='{request.Language}'>
                    <input type='hidden' name='client_ip' value='{request.Client_ip}'>
                    <input type='hidden' name='street' value='{request.Street}'>
                    <input type='hidden' name='street_n1' value='{request.Street_n1}'>
                    <input type='hidden' name='street_n2' value='{request.Street_n2}'>
                    <input type='hidden' name='addr2' value='{request.Addr2}'>
                    <input type='hidden' name='addr3' value='{request.Addr3}'>
                    <input type='hidden' name='city' value='{request.City}'>
                    <input type='hidden' name='postcode' value='{request.Postcode}'>
                    <input type='hidden' name='country' value='{request.Country}'>
                    <input type='hidden' name='email' value='{request.Email}'>
                    <input type='hidden' name='ba_firstname' value='{request.Ba_firstname}'>
                    <input type='hidden' name='ba_lastname' value='{request.Ba_lastname}'>
                    <input type='hidden' name='controlData' value='{request.ControlData}'>
                    <noscript>
                        <button type='submit'>Continue</button>
                    </noscript>
                </form>
            </body>
            </html>";

            return formHtml;
        }


        public string ProcessPayment(Guid orderId, decimal totalPrice, string userId)
        {
            var pos_id = _configuration["PaymentService:Pos_id"]
                ?? throw new Exception("PosId is not configured.");

            var key = _configuration["PaymentService:Key"]
                ?? throw new Exception("Key is not configured.");

            var currency = _configuration["PaymentService:Currency"]
                ?? throw new Exception("Currency is not configured.");

            var test = _configuration["PaymentService:Test"]
                ?? throw new Exception("Test is not configured.");

            var language = _configuration["PaymentService:Language"]
                ?? throw new Exception("Language is not configured.");

            var user = _context.Users.SingleOrDefault(u => u.Id == userId)
                ?? throw new Exception("User not found.");

            var serviceRequest = new ServiceRequest
            {
                Pos_id = pos_id,
                Order_id = orderId.ToString(),
                Session_id = Guid.NewGuid().ToString(),
                Amount = ((int)(totalPrice * 100)).ToString(),
                Currency = currency,
                Test = test,
                Language = language,
                Client_ip = "77.65.109.228",
                Street = user.Street,
                Street_n1 = user.Address,
                City = user.City,
                Postcode = user.PostalCode,
                Country = "PL",
                Email = user.Email,
                Ba_firstname = user.Name,
                Ba_lastname = user.Surname
            };

            string paramString = BuildParamString(serviceRequest);
            serviceRequest.ControlData = GenerateControlData(key, paramString);

            // Generate HTML form
            var formHtml = GeneratePaymentFormHtml(serviceRequest);

            return formHtml;
        }

        private string BuildParamString(ServiceRequest request)
        {
            var parameters = new List<string>();

            if (!string.IsNullOrEmpty(request.Pos_id)) parameters.Add($"pos_id={request.Pos_id}");
            if (!string.IsNullOrEmpty(request.Order_id)) parameters.Add($"order_id={request.Order_id}");
            if (!string.IsNullOrEmpty(request.Session_id)) parameters.Add($"session_id={request.Session_id}");
            if (!string.IsNullOrEmpty(request.Amount)) parameters.Add($"amount={request.Amount}");
            if (!string.IsNullOrEmpty(request.Currency)) parameters.Add($"currency={request.Currency}");
            if (!string.IsNullOrEmpty(request.Test)) parameters.Add($"test={request.Test}");
            if (!string.IsNullOrEmpty(request.Language)) parameters.Add($"language={request.Language}");
            if (!string.IsNullOrEmpty(request.Client_ip)) parameters.Add($"client_ip={request.Client_ip}");
            if (!string.IsNullOrEmpty(request.Street)) parameters.Add($"street={request.Street}");
            if (!string.IsNullOrEmpty(request.Street_n1)) parameters.Add($"street_n1={request.Street_n1}");
            if (!string.IsNullOrEmpty(request.Street_n2)) parameters.Add($"street_n2={request.Street_n2}");
            if (!string.IsNullOrEmpty(request.Addr2)) parameters.Add($"addr2={request.Addr2}");
            if (!string.IsNullOrEmpty(request.Addr3)) parameters.Add($"addr3={request.Addr3}");
            if (!string.IsNullOrEmpty(request.City)) parameters.Add($"city={request.City}");
            if (!string.IsNullOrEmpty(request.Postcode)) parameters.Add($"postcode={request.Postcode}");
            if (!string.IsNullOrEmpty(request.Country)) parameters.Add($"country={request.Country}");
            if (!string.IsNullOrEmpty(request.Email)) parameters.Add($"email={request.Email}");
            if (!string.IsNullOrEmpty(request.Ba_firstname)) parameters.Add($"ba_firstname={request.Ba_firstname}");
            if (!string.IsNullOrEmpty(request.Ba_lastname)) parameters.Add($"ba_lastname={request.Ba_lastname}");

            return string.Join("&", parameters);
        }

        private string GenerateControlData(string sharedKey, string paramString)
        {
            byte[] key = ConvertHexStringToBytes(sharedKey);
            return ComputeSha256Hash(paramString, key);
        }

        private static byte[] ConvertHexStringToBytes(string hexString)
        {
            int byteCount = hexString.Length / 2;
            byte[] bytes = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        private static string ComputeSha256Hash(string rawData, byte[] key)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] rawDataBytes = Encoding.UTF8.GetBytes(rawData);
                byte[] combinedData = new byte[rawDataBytes.Length + key.Length];
                Buffer.BlockCopy(rawDataBytes, 0, combinedData, 0, rawDataBytes.Length);
                Buffer.BlockCopy(key, 0, combinedData, rawDataBytes.Length, key.Length);

                byte[] bytes = sha256Hash.ComputeHash(combinedData);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


    }
}