namespace MedicalApp.BusinessLogic.DTOs.AdminDtos
{
    public class UsersPandingDto
    {
        public string UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string EmailStatus { get; set; } =string.Empty;
        public string Role { get; set; } =string.Empty;

        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
