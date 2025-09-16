namespace ContractMonthlyClaim.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public string Programme { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty; // Use string for visual-only prototype
        public decimal Amount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Default value
        public string Filename { get; set; } = string.Empty; // placeholder for upload
    }
}
