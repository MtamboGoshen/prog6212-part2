using ContractMonthlyClaim.Data;
using ContractMonthlyClaim.Services;
using ContractMonthlyClaimPrototype.Data;
using Microsoft.EntityFrameworkCore;

namespace ContractMonthlyClaim
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DBConnection"))
            );

            builder.Services.AddScoped<IClaimService, ClaimService>();

            var app = builder.Build();

            // Apply seeding
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                await SeedData.SeedAsyc(db);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

