namespace MedicalApp.BusinessLogic.DTOs.AuthenticationDto
{
    public class ForgetPasswordDto
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
    }
}
