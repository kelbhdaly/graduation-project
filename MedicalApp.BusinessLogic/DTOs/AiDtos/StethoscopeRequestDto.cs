namespace MedicalApp.BusinessLogic.DTOs.AiDtos
{
    public class StethoscopeRequestDto
    {
        public IFormFile Audio { get; set; }

        public string? PatientId { get; set; }
    }
}
