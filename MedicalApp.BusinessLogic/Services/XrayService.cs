namespace MedicalApp.BusinessLogic.Services
{
    public class XrayService : IXrayService
    {
        private readonly IGenericAiClient _aiClient;
        private readonly IImageService _imageService;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public XrayService(
            IGenericAiClient aiClient,
            IImageService imageService,
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _aiClient = aiClient;
            _imageService = imageService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<XrayResponseDto> AnalyzeAsync(IFormFile image)
        {
            // save image
            var imageUrl = (await _imageService
                .UploadImagesAsync(new List<IFormFile> { image }, "xrays"))
                .First();


            if (string.IsNullOrEmpty(imageUrl))
                throw new Exception("Failed to upload image");

            //call AI
            var result = await _aiClient.PostImageAsync<XrayApiResult>(
                "https://ahmed99a-xray-api.hf.space/predict",
                image);


            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var request = _httpContextAccessor.HttpContext.Request;
            var fullUrl = $"{request.Scheme}://{request.Host}{imageUrl}";
            var entity = new XrayAnalysis
            {
                ImageUrl = fullUrl,
                PredictedClass = result.Predicted_Class,
                Confidence = result.Confidence,
                LungOpacity = result.Class_Probabilities["Lung Opacity"],
                Normal = result.Class_Probabilities["Normal"],
                ViralPneumonia = result.Class_Probabilities["Viral Pneumonia"],
                UserId = userId
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();

            return new XrayResponseDto
            {
                ImageUrl = fullUrl,
                PredictedClass = result.Predicted_Class,
                Confidence = result.Confidence,
                ClassProbabilities = result.Class_Probabilities
            };
        }



        public async Task<List<XrayHistoryDto>> GetHistoryAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var data = await _context.XrayAnalysisResults
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new XrayHistoryDto
                {
                    CreatedAt = x.CreatedAt,
                    ImageUrl = x.ImageUrl,
                    Result = x.PredictedClass,
                    Confidence = x.Confidence,

                    LungOpacity = x.LungOpacity,
                    Normal = x.Normal,
                    ViralPneumonia = x.ViralPneumonia
                })
                .ToListAsync();

            return data;
        }

    }
}
