namespace MedicalApp.DataAccess.Data.Models
{
    public class CoughAnalysis
    {
        public int Id { get; set; }

        public string AudioUrl { get; set; } = string.Empty;

        public string SupportLabel { get; set; } = string.Empty;

        public double RiskScore { get; set; }

        public string ClinicalUse { get; set; } = string.Empty;

        public string Disclaimer { get; set; } = string.Empty;

        public double CovidProbability { get; set; }
        public double NotCovidProbability { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
