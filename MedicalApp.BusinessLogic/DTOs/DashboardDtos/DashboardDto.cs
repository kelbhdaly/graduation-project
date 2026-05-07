namespace MedicalApp.BusinessLogic.DTOs.DashboardDtos
{
    public class DashboardDto
    {
        public LungRiskDashboardDto? LungRisk { get; set; }
        public CoughDashboardDto? Cough { get; set; }
        public XrayDashboardDto? Xray { get; set; }
        public StethoscopeDashboardDto? Stethoscope { get; set; }

    }
}
