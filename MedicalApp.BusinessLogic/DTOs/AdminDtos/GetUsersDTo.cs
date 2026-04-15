namespace MedicalApp.BusinessLogic.DTOs.AdminDtos
{
    public class GetUsersDTo
    {
        public string UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Role { get; set; }
        public string Status { get; set; }
        public string EmailStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
