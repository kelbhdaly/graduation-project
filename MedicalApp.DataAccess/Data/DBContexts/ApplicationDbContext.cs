using System.Reflection;

namespace MedicalApp.DataAccess.Data.DBContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
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
        public DbSet<CoughAnalysis> CoughAnalyses { get; set; }
        public DbSet<StethoscopeAnalysis> StethoscopeAnalyses { get; set; }
        #endregion

    }
}
