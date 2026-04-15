using MedicalApp.BusinessLogic.Ai;

namespace MedicalApp.BusinessLogic.Services
{
    public class LungRiskService :ILungRiskService
    {
        private readonly IGenericAiClient _aiClient;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LungRiskService(
            IGenericAiClient aiClient,
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _aiClient = aiClient;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

     

        public async Task<LungRiskResponseDto> AnalyzeAsync(LungRiskRequestDto lungRiskRequestDto)
        {

            var request = new LungRiskApiRequest
            {
                Features = new FeaturesModel
                {
                    obesity = lungRiskRequestDto.Obesity,
                    coughing_of_blood = lungRiskRequestDto.CoughingOfBlood,
                    alcohol_use = lungRiskRequestDto.AlcoholUse,
                    dust_allergy = lungRiskRequestDto.DustAllergy,
                    passive_smoker = lungRiskRequestDto.PassiveSmoker,
                    balanced_diet = lungRiskRequestDto.BalancedDiet,
                    genetic_risk = lungRiskRequestDto.GeneticRisk,
                    occupational_hazards = lungRiskRequestDto.OccupationalHazards,
                    chest_pain = lungRiskRequestDto.ChestPain,
                    air_pollution = lungRiskRequestDto.AirPollution
                }
            };

            //  call AI
            var result = await _aiClient.PostJsonAsync<LungRiskApiRequest, LungRiskApiResponse>(
                "https://ahmed99a-resk-api.hf.space/predict",
                request);
            string? userId = GetUserId();

            //  save DB
            var entity = new LungRiskAnalysis
            {
                Obesity = lungRiskRequestDto.Obesity,
                CoughingOfBlood = lungRiskRequestDto.CoughingOfBlood,
                AlcoholUse = lungRiskRequestDto.AlcoholUse,
                DustAllergy = lungRiskRequestDto.DustAllergy,
                PassiveSmoker = lungRiskRequestDto.PassiveSmoker,
                BalancedDiet = lungRiskRequestDto.BalancedDiet,
                GeneticRisk = lungRiskRequestDto.GeneticRisk,
                OccupationalHazards = lungRiskRequestDto.OccupationalHazards,
                ChestPain = lungRiskRequestDto.ChestPain,
                AirPollution = lungRiskRequestDto.AirPollution,

                Result = result.Predicted_Class_Name,
                Index = result.Predicted_Class_Index,

                Low = result.Probabilities["Low"],
                Medium = result.Probabilities["Medium"],
                High = result.Probabilities["High"],

                UserId = userId
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();

            // response
            return new LungRiskResponseDto
            {
                Result = result.Predicted_Class_Name,
                Index = result.Predicted_Class_Index,

                Low = Math.Round(result.Probabilities["Low"], 2),
                Medium = Math.Round(result.Probabilities["Medium"], 2),
                High = Math.Round(result.Probabilities["High"], 2)
            };
        }


        public async Task<List<LungRiskHistoryDto>> GetHistoryAsync()
        {
            var userId = GetUserId();

            var data = await _context.LungRiskAnalyses
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new LungRiskHistoryDto
                {
                    CreatedAt = x.CreatedAt,

                    Obesity = x.Obesity,
                    CoughingOfBlood = x.CoughingOfBlood,
                    AlcoholUse = x.AlcoholUse,
                    DustAllergy = x.DustAllergy,
                    PassiveSmoker = x.PassiveSmoker,
                    BalancedDiet = x.BalancedDiet,
                    GeneticRisk = x.GeneticRisk,
                    OccupationalHazards = x.OccupationalHazards,
                    ChestPain = x.ChestPain,
                    AirPollution = x.AirPollution,

                    Result = x.Result,

                    Low = x.Low,
                    Medium = x.Medium,
                    High = x.High
                })
                .ToListAsync();

            return data;
        }
        private string? GetUserId()
        {
            return _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
