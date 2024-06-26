using backend.Models;

namespace backend.Interfaces
{
    public interface IEmailService
    {
        Task SendActivationEmail(string email, string userId, string name, string token);
        Task SendOrderConfirmationEmail(string email, string name, OrderDTO order);
        Task SendNewOrderNotificationToOwner(OrderDTO order, UserModel user);
        Task SendOrderStatusChangeEmail(string email, string name, OrderDTO order);
    }
}