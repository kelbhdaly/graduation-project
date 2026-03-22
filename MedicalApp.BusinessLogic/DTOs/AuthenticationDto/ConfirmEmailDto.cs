namespace MedicalApp.BusinessLogic.DTOs.AuthenticationDto
{
    public class ConfirmEmailDTO
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
