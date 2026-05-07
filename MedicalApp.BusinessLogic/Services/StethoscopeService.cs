namespace MedicalApp.BusinessLogic.Services
{
    public class StethoscopeService : IStethoscopeService
    {
        private readonly IGenericAiClient _aiClient;
        private readonly IFileService _fileService;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StethoscopeService(IGenericAiClient aiClient, IFileService fileService,
            ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _aiClient = aiClient;
            _fileService = fileService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<StethoscopeResponseDto> AnalyzeAsync(StethoscopeRequestDto stethoscopeRequestDto)
        {

            var doctorId = UserId();

            if (doctorId == null)
                throw new UnauthorizedAccessException();

            var patientId = string.IsNullOrEmpty(stethoscopeRequestDto.PatientId) ? doctorId
                                                  : stethoscopeRequestDto.PatientId;


            // Validate the uploaded file
            var extension = Path.GetExtension(stethoscopeRequestDto.Audio.FileName).ToLower();
            if (extension != ".wav")
                throw new Exception("Stethoscope model accepts only WAV");


            var audioUrl = (await _fileService
           .UploadAudioAsync(new List<IFormFile> { stethoscopeRequestDto.Audio }, "stethoscope")).First();

            var fullUrl = FullAudioUrl(audioUrl);



            //Call the AI model API to analyze the audio
            var result = await _aiClient.PostFileAsync<StethoscopeApiResponse>
                ("https://ahmed99a-audio-model-api.hf.space/predict", stethoscopeRequestDto.Audio);


            var entity = new StethoscopeAnalysis
            {
                AudioUrl = audioUrl,
                Result = result.PredictedClass,
                Confidence = result.Confidence,
                DoctorId = doctorId,
                PatientId = patientId
            };
            _context.Add(entity);
            await _context.SaveChangesAsync();

            return new StethoscopeResponseDto
            {
                AudioUrl = fullUrl,
                Result = result.PredictedClass,
                Confidence = result.Confidence,
                PatientId = patientId
            };
        }

        private string FullAudioUrl(string audioUrl)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fullUrl = baseUrl + audioUrl;
            return fullUrl;
        }

        public async Task<List<HistoryStethoscopeAnalysisDto>> GetAnalysisByPatientIdAsync()
        {
            var patientId = UserId();
            var data = await _context.StethoscopeAnalyses.Where(s => s.PatientId == patientId)
                .OrderByDescending(s => s.CreatedAt).ToListAsync();

            return data.Select(s => new HistoryStethoscopeAnalysisDto
            {
                AudioUrl = FullAudioUrl(s.AudioUrl),
                Result = s.Result,
                Confidence = s.Confidence,
                AnalyzedAt = s.CreatedAt
            }).ToList();
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
        #endregion
    }
}
