namespace MedicalApp.BusinessLogic.DTOs.AdminDtos
{
    public class UsersPandingDto
    {
        public string UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Status { get; set; }
    }
}
