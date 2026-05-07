namespace MedicalApp.BusinessLogic.DTOs.DashboardDtos
{
    public class CoughDashboardDto
    {
        public string AudioUrl { get; set; } = string.Empty;
    
        public string SupportLabel { get; set; } = string.Empty;

        public double RiskScore { get; set; }

        public double CovidProbability { get; set; }
        public double NormalProbability { get; set; }

        public string ClinicalUse { get; set; } = string.Empty;
    }
}
