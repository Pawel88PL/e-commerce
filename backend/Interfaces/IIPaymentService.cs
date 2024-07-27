using backend.Models;

namespace backend.Interfaces
{
    public interface IPaymentService
    {
        string ProcessPayment(Guid orderId, decimal totalPrice, string userId);
    }
}