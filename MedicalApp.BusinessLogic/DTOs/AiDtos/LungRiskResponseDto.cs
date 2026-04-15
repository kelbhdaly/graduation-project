namespace MedicalApp.BusinessLogic.DTOs.AiDtos
{
    public class LungRiskResponseDto
    {
        public string Result { get; set; } = string.Empty;
        public int Index { get; set; }

        public double Low { get; set; }
        public double Medium { get; set; }
        public double High { get; set; }
    }
}
