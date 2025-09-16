using ContractMonthlyClaim.Data;
using ContractMonthlyClaim.Models;
using Microsoft.EntityFrameworkCore;
using ContractMonthlyClaim.Services;

namespace ContractMonthlyClaim.Services
{
    public class ClaimService : IClaimService
    {
        private readonly ApplicationDBContext applicationDBContext;

        public ClaimService(ApplicationDBContext context) => applicationDBContext = context;

        public async Task<int> AddClaim(Claim claim)
        {
            await applicationDBContext.Claims.AddAsync(claim);
            await applicationDBContext.SaveChangesAsync(); // Corrected
            return claim.Id;
        }

        public async Task<bool> DeleteClaim(int claimId)
        {
            var claim = await applicationDBContext.Claims.FirstOrDefaultAsync(x => x.Id == claimId);
            if (claim != null)
            {
                applicationDBContext.Remove(claim);
                await applicationDBContext.SaveChangesAsync(); // Corrected
                return true;
            }
            return false;
        }

        public async Task<List<Claim>> GetClaims()
        {
            return await applicationDBContext.Claims.ToListAsync();
        }
    }
}