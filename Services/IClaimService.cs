using ContractMonthlyClaim.Models;

namespace ContractMonthlyClaim.Services
{
    public interface IClaimService
    {
        Task<List<Claim>> GetClaims();
        Task<int> AddClaim(Claim claim);
        Task<bool> DeleteClaim(int claimId);
    }
}
