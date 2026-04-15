namespace MedicalApp.DataAccess.Data.Models
{
    public class XrayAnalysis
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; } = default!;

        public string PredictedClass { get; set; } = default!;

        public double Confidence { get; set; }

        public double LungOpacity { get; set; }

        public double Normal { get; set; }

        public double ViralPneumonia { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
