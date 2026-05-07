namespace MedicalApp.BusinessLogic.DTOs.AiDtos
{
    public class StethoscopeResponseDto
    {
        public string AudioUrl { get; set; }

        public string Result { get; set; }

        public double Confidence { get; set; }

        public string PatientId { get; set; }
    }
}
