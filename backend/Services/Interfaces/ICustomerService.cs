using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> AddNewCustomer(Customer customer);
        Task<Customer?> GetCustomerAsync(Guid id);
    }
}