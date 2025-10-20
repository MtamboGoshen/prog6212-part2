using System.ComponentModel.DataAnnotations;

namespace ContractMonthlyClaim.Models
{
    public class SubmitClaimViewModel
    {
        [Required]
        public string LecturerName { get; set; } = string.Empty;

        [Required]
        public string Programme { get; set; } = string.Empty;

        [Required]
        public string Month { get; set; } = string.Empty;

        [Required]
        [Range(0.1, 1000)]
        public decimal HoursWorked { get; set; }

        [Required]
        [Range(1, 10000)]
        public decimal HourlyRate { get; set; }

        public string Notes { get; set; } = string.Empty;

        // This property will handle the file upload via the form
        public IFormFile? Document { get; set; }
    }
}