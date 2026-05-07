namespace MedicalApp.BusinessLogic.DTOs.DashboardDtos
{
    public class XrayDashboardDto
    {
        public string ImageUrl { get; set; }

        public string Result { get; set; }

        public double Confidence { get; set; }

        public double LungOpacity { get; set; }
        public double Normal { get; set; }
        public double ViralPneumonia { get; set; }
    }
}
