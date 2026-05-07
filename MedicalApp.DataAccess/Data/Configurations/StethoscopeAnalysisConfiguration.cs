
namespace MedicalApp.DataAccess.Data.Configurations
{
    internal class StethoscopeAnalysisConfiguration : IEntityTypeConfiguration<StethoscopeAnalysis>
    {
        public void Configure(EntityTypeBuilder<StethoscopeAnalysis> builder)
        {
            builder.HasOne(s => s.Doctor)
                .WithMany()
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s => s.Patient)
                .WithMany().HasForeignKey(S => S.PatientId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
