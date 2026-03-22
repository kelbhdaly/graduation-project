namespace MedicalApp.BusinessLogic.DTOs.AuthenticationDto
{
    public class LoginDto
    {

        [DataType(DataType.EmailAddress)]

        public string Email { get; set; } = default!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}
