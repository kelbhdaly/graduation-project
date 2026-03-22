namespace MedicalApp.BusinessLogic.DTOs.AuthenticationDto
{
    public class CreateUserDto
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Password Not Same")]
        [Required]
        public string ConfirmPassword { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
}
