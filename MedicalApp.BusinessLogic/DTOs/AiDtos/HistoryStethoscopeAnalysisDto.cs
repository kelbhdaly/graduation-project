namespace MedicalApp.BusinessLogic.DTOs.AiDtos
{
    public class HistoryStethoscopeAnalysisDto
    {
        public string AudioUrl { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public DateTime AnalyzedAt { get; set; }
    }
}
