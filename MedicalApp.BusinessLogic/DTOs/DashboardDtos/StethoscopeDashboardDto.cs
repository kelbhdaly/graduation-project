namespace MedicalApp.BusinessLogic.DTOs.DashboardDtos
{
    public class StethoscopeDashboardDto
    {
        public string AudioUrl { get; set; } = string.Empty;

        public string Result { get; set; } = string.Empty;

        public double Confidence { get; set; }

     
    }
}
