namespace backend.Interfaces
{
    public interface ITokenService
    {
        Task RevokeTokenAsync(string token);
        Task<bool> IsTokenRevokedAsync(string token);
    }
}
