namespace MedicalApp.BusinessLogic.DTOs.AuthenticationDto
{
    public class ResetPasswordDto
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string NewPassword { get; set; } = default!;

    }
}
