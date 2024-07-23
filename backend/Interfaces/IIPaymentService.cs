using backend.Models;

namespace backend.Interfaces
{
    public interface IPaymentService
    {
        string GeneratePaymentFormHtml(ServiceRequest request);
        string ProcessPayment(Guid orderId, decimal totalPrice, string userId);
    }
}