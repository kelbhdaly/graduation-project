namespace MedicalApp.BusinessLogic.DTOs.AuthenticationDto
{
    public class VerifyOtpDto
    {
        public string Email { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
