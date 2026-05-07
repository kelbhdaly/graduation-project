namespace MedicalApp.BusinessLogic.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public DashboardService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            var userId = UserId();

            return await GetResponseDataFromDatabaseToShowInDashboardAsync(userId);
        }


        public async Task<DashboardDto> GetDashboardByDoctorAsync(string userId)
        {
            var patient = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId );
            if (patient == null)
                throw new NotFoundUserException($"Not Found Patient has Id = {userId}");

            return await GetResponseDataFromDatabaseToShowInDashboardAsync(patient.Id);


        }



        #region Private Method

        private string UserId()
        {
            var userId = _httpContextAccessor.HttpContext.User
                          .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            return userId;
        }



        private async Task<DashboardDto> GetResponseDataFromDatabaseToShowInDashboardAsync(string userId)
        {
            var lung = await _dbContext.LungRiskAnalyses
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            var cough = await _dbContext.CoughAnalyses
                   .Where(x => x.UserId == userId)
                   .OrderByDescending(x => x.CreatedAt)
                   .FirstOrDefaultAsync();


            var xray = await _dbContext.XrayAnalysisResults
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            var sethoscope = await _dbContext.StethoscopeAnalyses
                .Where(x => x.PatientId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            return new DashboardDto
            {
                LungRisk = lung != null ? new LungRiskDashboardDto
                {

                    AirPollution = lung.AirPollution,
                    BalancedDiet = lung.BalancedDiet,
                    ChestPain = lung.ChestPain,
                    CoughingOfBlood = lung.CoughingOfBlood,
                    DustAllergy = lung.DustAllergy,
                    GeneticRisk = lung.GeneticRisk,
                    Obesity = lung.Obesity,
                    OccupationalHazards = lung.OccupationalHazards,
                    PassiveSmoker = lung.PassiveSmoker,
                    

                    //Reasult
                    High = lung.High,
                    Medium = lung.Medium,
                    Low = lung.Low,
                    Result = lung.Result

                } : null,

                Cough = cough == null ? null : new CoughDashboardDto
                {
                    RiskScore = cough.RiskScore,
                    CovidProbability = cough.CovidProbability,
                    NormalProbability = cough.NotCovidProbability,
                    AudioUrl = FullAudioUrl(cough.AudioUrl)
                },


                Xray = xray == null ? null : new XrayDashboardDto
                {
                    Result = xray.PredictedClass,
                    Confidence = xray.Confidence,
                    ImageUrl = xray.ImageUrl,
                    LungOpacity = xray.LungOpacity, 
                    Normal = xray.Normal,
                    ViralPneumonia = xray.ViralPneumonia
                },


                Stethoscope = sethoscope == null ? null : new StethoscopeDashboardDto
                {
                    Result = sethoscope.Result,
                    Confidence = sethoscope.Confidence,
                    AudioUrl = FullAudioUrl(sethoscope.AudioUrl)
                },

            };
        }


        private string FullAudioUrl(string audioUrl)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fullUrl = baseUrl + audioUrl;
            return fullUrl;
        }



        #endregion




    }
}
