namespace MedicalApp.BusinessLogic.Services
{
    public class CoughService : ICoughService
    {
        private readonly IGenericAiClient _aiClient;
        private readonly IFileService _fileService;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CoughService(
            IGenericAiClient aiClient,
            IFileService fileService,
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _aiClient = aiClient;
            _fileService = fileService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CoughResponseDto> AnalyzeAsync(IFormFile audio)
        {
            var audioUrl = (await _fileService
            .UploadAudioAsync(new List<IFormFile> { audio }, "cough"))
            .First();
            string fullAudioUrl = AudioFullUrlToShow(audioUrl);

            var result = await _aiClient.PostFileAsync<CoughApiResponse>(
            "https://ahmed99a-cough-audio-fastapi.hf.space/predict",
            audio);

            var userId = UserId();


            // 3. extract probabilities
            var covid = result.ClassProbabilities.ContainsKey("covid")
                           ? result.ClassProbabilities["covid"] : 0;

            var notCovid = result.ClassProbabilities.ContainsKey("not_covid")
                           ? result.ClassProbabilities["not_covid"] : 0;

            var cough = new CoughAnalysis
            {
                AudioUrl = audioUrl,
                SupportLabel = result.SupportLabel,
                RiskScore = result.RiskScore,
                ClinicalUse = result.ClinicalUse,
                Disclaimer = result.Disclaimer,
                CovidProbability = covid,
                NotCovidProbability = notCovid,
                UserId = userId
            };

            _context.Add(cough);
            await _context.SaveChangesAsync();

            return new CoughResponseDto
            {
                AudioUrl = fullAudioUrl,
                SupportLabel = result.SupportLabel,
                RiskScore = result.RiskScore,
                ClinicalUse = result.ClinicalUse,
                CovidProbability = covid,
                NormalProbability = notCovid
            };
        }


        public async Task<List<CoughHistoryDto>> GetHistoryAsync()
        {
            var userId = UserId();

            var data = await _context.CoughAnalyses
                         .Where(user => user.UserId == userId)
                         .OrderByDescending(x => x.CreatedAt).ToListAsync();



            var history = data.Select(x => new CoughHistoryDto
            {
                DateTime = x.CreatedAt,
                AudioUrl = AudioFullUrlToShow(x.AudioUrl),
                SupportLabel = x.SupportLabel,
                RiskScore = x.RiskScore,
                CovidProbability = x.CovidProbability,
                NormalProbability = x.NotCovidProbability,
                ClinicalUse = x.ClinicalUse
            }).ToList();
            return history;
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



        private string AudioFullUrlToShow(string audioUrl)
        {
            var request = _httpContextAccessor.HttpContext.Request;

            var baseUrl = $"{request.Scheme}://{request.Host}";

            var fullAudioUrl = baseUrl + audioUrl;
            return fullAudioUrl;
        }
        #endregion
    }
}
