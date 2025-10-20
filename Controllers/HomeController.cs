using System.Diagnostics;
using ContractMonthlyClaim.Models;
using ContractMonthlyClaim.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ContractMonthlyClaim.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClaimService _claimService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IClaimService claimService, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _claimService = claimService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var claims = await _claimService.GetClaims();
            return View(claims);
        }

        [Authorize(Roles = "Manager, Programme Coordinator")]
        public async Task<IActionResult> ApproverQueue()
        {
            var claims = await _claimService.GetPendingClaims();
            return View(claims);
        }

        [Authorize(Roles = "Lecturer")]
        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile document)
        {
            if (ModelState.IsValid)
            {
                claim.Amount = claim.HoursWorked * claim.HourlyRate;

                if (document != null && document.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + document.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(fileStream);
                    }
                    claim.Filename = uniqueFileName;
                }

                await _claimService.AddClaim(claim);
                return RedirectToAction("Index");
            }
            return View(claim);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Programme Coordinator")]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            await _claimService.UpdateClaimStatus(claimId, "Approved");
            return RedirectToAction("ApproverQueue");
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Programme Coordinator")]
        public async Task<IActionResult> RejectClaim(int claimId)
        {
            await _claimService.UpdateClaimStatus(claimId, "Rejected");
            return RedirectToAction("ApproverQueue");
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Programme Coordinator")]
        public async Task<IActionResult> DeleteClaim(int claimId)
        {
            await _claimService.DeleteClaim(claimId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var claim = await _claimService.GetClaimById(id);
            if (claim == null)
            {
                return View("NotFound");
            }
            return View(claim);
        }

        [Authorize(Roles = "Manager, Programme Coordinator")]
        public async Task<IActionResult> Edit(int id)
        {
            var claim = await _claimService.GetClaimById(id);
            if (claim == null)
            {
                return View("NotFound");
            }
            return View(claim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Programme Coordinator")]
        public async Task<IActionResult> Edit(Claim claim)
        {
            if (ModelState.IsValid)
            {
                claim.Amount = claim.HoursWorked * claim.HourlyRate;
                await _claimService.UpdateClaim(claim);
                return RedirectToAction(nameof(Index));
            }
            return View(claim);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}