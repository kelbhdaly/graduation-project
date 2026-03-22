namespace MedicalApp.Presentation.Extensions
{
    public static class UtiltiesRegisteration
    {
        public static void AddUtilities(this IServiceCollection services)
        {
            services.AddScoped<IImageService,ImageService >();
        }
    }
}
