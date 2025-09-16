using System.Diagnostics;
using ContractMonthlyClaim.Models;
using ContractMonthlyClaim.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractMonthlyClaim.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IClaimService _claimService;
        public HomeController(ILogger<HomeController> logger, IClaimService claimService)
        {
            _logger = logger;
            _claimService = claimService;
        }

        public async Task<IActionResult> Index()
        {
            var claims = await _claimService.GetClaims();
            return View(claims);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}