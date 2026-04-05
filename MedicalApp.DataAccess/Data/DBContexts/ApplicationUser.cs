namespace MedicalApp.DataAccess.Data.DBContexts
{
    public class ApplicationUser : IdentityUser
    {
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = new();
        public UserStatus UserStatus { get; set; } = UserStatus.Pending;
    }
}
