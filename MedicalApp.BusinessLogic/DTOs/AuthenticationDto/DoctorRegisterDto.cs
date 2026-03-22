namespace MedicalApp.BusinessLogic.DTOs.AuthenticationDto
{
    public class DoctorRegisterDto :RegisterDto
    {
        public string MedicalLicense { get; set; } = default!;
    }
}
