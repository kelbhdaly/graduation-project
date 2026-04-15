namespace MedicalApp.Presentation.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task AddSeedDataBaseAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();

            var ObjectOfDataSeeding = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjectOfDataSeeding.IdentityDataSeedAsync();



        }
    }
}
