namespace MedicalApp.BusinessLogic.IServices
{
    public interface ILungRiskService
    {
        Task<LungRiskResponseDto> AnalyzeAsync(LungRiskRequestDto lungRiskRequestDto);
        public Task<List<LungRiskHistoryDto>> GetHistoryAsync();
    }
}
