namespace MedicalApp.BusinessLogic.ExtensionMethod
{
    public static class BusinessLogicServices
    {
        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<PostImageUrlResolver>();
            services.AddAutoMapper(cfg =>
            {
                
            } , typeof(DoctorProfile).Assembly);
            services.AddScoped<IFavoritePostService, FavoritePostService>();
            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();  
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<ILungRiskService, LungRiskService>();

        }
    }
}