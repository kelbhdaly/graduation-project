namespace MedicalApp.BusinessLogic.DTOs.AiDtos
{
    public class XrayHistoryDto
    {
        public DateTime CreatedAt { get; set; }

        public string ImageUrl { get; set; }

        public string Result { get; set; }

        public double Confidence { get; set; }

        public double LungOpacity { get; set; }
        public double Normal { get; set; }
        public double ViralPneumonia { get; set; }
    }
}
