using Microsoft.EntityFrameworkCore;
using ContractMonthlyClaim.Models;
using ContractMonthlyClaim.Data;


namespace ContractMonthlyClaimPrototype.Data
{
    public class SeedData
    {
        public static async Task SeedAsyc(ApplicationDBContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.Claims.AnyAsync())
            {
                context.Claims.AddRange(
                    new Claim { LecturerName = "Dr. John Doe", Programme = "IT", Month = "2025-12", Amount = 500, Notes = "Research hours", Status = "Pending" },
                    new Claim { LecturerName = "Dr. Goshen Mtambo", Programme = "PE", Month = "2025-07", Amount = 500, Notes = "Hockey", Status = "Pending" },
                    new Claim { LecturerName = "Dr. Tracy Morgan", Programme = "CS", Month = "2025-08", Amount = 750, Notes = "Extra teaching", Status = "Approved" },                    
                    new Claim { LecturerName = "Dr. Cristiano Ronaldo", Programme = "FC", Month = "2025-02", Amount = 750, Notes = "Football", Status = "Approved" }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
