namespace MedicalApp.BusinessLogic.DTOs.AiDtos
{
    public class CoughHistoryDto
    {
        public DateTime DateTime { get; set; }
        public string AudioUrl { get; set; }

        public string SupportLabel { get; set; }

        public double RiskScore { get; set; }

        public double CovidProbability { get; set; }
        public double NormalProbability { get; set; }

        public string ClinicalUse { get; set; }
    }
}
