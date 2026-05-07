namespace MedicalApp.BusinessLogic.IServices
{
    public interface ICoughService
    {
        public Task<CoughResponseDto> AnalyzeAsync(IFormFile audio);
        public Task<List<CoughHistoryDto>> GetHistoryAsync();
    }
}
