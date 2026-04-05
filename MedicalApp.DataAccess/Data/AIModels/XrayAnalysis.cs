namespace MedicalApp.DataAccess.Data.AIModels
{
    public class XrayAnalysis
    {
        public int Id { get; set; }

        public string InputImagePath { get; set; } = default!;
        public string OutputImagePath { get; set; } = default!;

        public string Diagnosis { get; set; } = default!; 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
