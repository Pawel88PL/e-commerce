namespace MiodOdStaniula.Services.Interfaces
{
    public interface ITotalCostService
    {
        Task<decimal> CalculateShippingCostAsync(Guid cartId);
        Task<decimal> CalculateProductCostAsync(Guid cartId);
        Task<decimal> CalculateTotalAsync(Guid cartId);
    }
}
