namespace MedicalApp.BusinessLogic.DTOs.AuthenticationDto
{
    public class RegisterDto : CreateUserDto
    {
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;
        [Required]

        public string NationalId { get; set; } = default!;

       

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public IsMarried IsMarried { get; set; }

        public Governorate Governorate { get; set; }

    }
}
