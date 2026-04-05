namespace MedicalApp.DataAccess.Data.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = null!;
        public DateTime ExpiresOn { get; set; }
        public bool IsRevoked { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
