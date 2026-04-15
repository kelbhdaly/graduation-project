namespace MedicalApp.Presentation.Extensions
{
    public static class AiRegisteration
    {
        public static void AddAiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IXrayAiClient, XrayAiClient>(client =>
            {
                client.BaseAddress = new Uri("https://ahmed99a-xray-api.hf.space");
            });
            services.AddHttpClient<IGenericAiClient, GenericAiClient>();
            services.AddScoped<IXrayService, XrayService>();
        }
    }
}
