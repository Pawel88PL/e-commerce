using backend.Models;

namespace backend.Interfaces
{
    public interface IPaymentService
    {
        string GeneratePaymentFormHtml(ServiceRequest request);
        ServiceRequest ProcessPayment(Guid orderId, decimal totalPrice, string userId);
    }
}