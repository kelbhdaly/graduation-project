namespace MedicalApp.DataAccess.Data.DBContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }

        #region Tables In DataBase

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<FavoritePost> FavoritePosts { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }
        public DbSet<XrayAnalysis> XrayAnalysisResults { get; set; }
        public DbSet<LungRiskAnalysis> LungRiskAnalyses { get; set; }
        #endregion

    }
}
