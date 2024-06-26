using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Interfaces;
using backend.Models;

namespace backend.Services
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _context;

        public TokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RevokeTokenAsync(string token)
        {
            var revokedToken = new RevokedToken
            {
                Token = token,
                RevokedAt = DateTime.UtcNow
            };
            _context.RevokedTokens.Add(revokedToken);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenRevokedAsync(string token)
        {
            return await _context.RevokedTokens.AnyAsync(t => t.Token == token);
        }
    }
}
