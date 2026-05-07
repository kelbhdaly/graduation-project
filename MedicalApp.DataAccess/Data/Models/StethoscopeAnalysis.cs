namespace MedicalApp.DataAccess.Data.Models
{
    public class StethoscopeAnalysis
    {

        public int Id { get; set; }

        public string AudioUrl { get; set; } = string.Empty;

        public string Result { get; set; } = string.Empty;

        public double Confidence { get; set; }

        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }

        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
