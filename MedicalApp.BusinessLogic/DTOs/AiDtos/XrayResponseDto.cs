namespace MedicalApp.BusinessLogic.DTOs.AiDtos
{
    public class XrayResponseDto
    {
        public string ImageUrl { get; set; }
        public string PredictedClass { get; set; }
        public double Confidence { get; set; }
        public Dictionary<string, double> ClassProbabilities { get; set; }
    }
}
