using backend.Models;

namespace backend.Interfaces
{
    public interface IOrderConfirmationService
    {
        string GenerateOrderConfirmationURL(int code);
        Task<bool> OrderConfirmation(ServiceResponse serviceResponse);
    }
}