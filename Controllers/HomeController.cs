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
        private readonly IEncryptionService _encryptionService; // Service for encryption

        // Updated constructor to inject the encryption service
        public HomeController(ILogger<HomeController> logger, IClaimService claimService, IWebHostEnvironment webHostEnvironment, IEncryptionService encryptionService)
        {
            _logger = logger;
            _claimService = claimService;
            _webHostEnvironment = webHostEnvironment;
            _encryptionService = encryptionService;
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
            return View(new SubmitClaimViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> SubmitClaim(SubmitClaimViewModel viewModel)
        {
            if (viewModel.Document != null)
            {
                var maxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
                if (viewModel.Document.Length > maxFileSizeInBytes)
                {
                    ModelState.AddModelError("Document", "The file size cannot exceed 5 MB.");
                }

                var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                var fileExtension = Path.GetExtension(viewModel.Document.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("Document", "Invalid file type. Only .pdf, .docx, and .xlsx are allowed.");
                }
            }

            if (ModelState.IsValid)
            {
                var newClaim = new Claim
                {
                    LecturerName = viewModel.LecturerName,
                    Programme = viewModel.Programme,
                    Month = viewModel.Month,
                    HoursWorked = viewModel.HoursWorked,
                    HourlyRate = viewModel.HourlyRate,
                    Notes = viewModel.Notes,
                    Amount = viewModel.HoursWorked * viewModel.HourlyRate
                };

                if (viewModel.Document != null && viewModel.Document.Length > 0)
                {
                    // 1. Encrypt the file's content
                    var encryptedData = await _encryptionService.EncryptFileAsync(viewModel.Document);

                    // 2. Save the encrypted data to a file
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.Document.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    await System.IO.File.WriteAllBytesAsync(filePath, encryptedData);
                    newClaim.Filename = uniqueFileName;
                }

                await _claimService.AddClaim(newClaim);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // NEW ACTION: To decrypt and view documents securely
        public async Task<IActionResult> ViewDocument(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return NotFound();
            }

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", filename);
            if (!System.IO.File.Exists(filePath))
            {
                return View("NotFound");
            }

            // 1. Read the encrypted file from disk
            var encryptedData = await System.IO.File.ReadAllBytesAsync(filePath);

            // 2. Decrypt the data
            var decryptedData = await _encryptionService.DecryptFileAsync(encryptedData);

            // 3. Determine the correct content type for the browser
            var contentType = "application/octet-stream";
            var fileExtension = Path.GetExtension(filename).ToLowerInvariant();
            if (fileExtension == ".pdf") contentType = "application/pdf";
            else if (fileExtension == ".docx") contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            else if (fileExtension == ".xlsx") contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // 4. Return the decrypted file to the user
            return File(decryptedData, contentType, Path.GetFileName(filename)); // Use a safe filename
        }

        // ... (The rest of your controller methods are unchanged) ...

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