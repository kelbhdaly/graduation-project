namespace MedicalApp.BusinessLogic.IServices
{
    public interface IXrayService
    {
        public Task<XrayResponseDto> AnalyzeAsync(IFormFile image);
        public Task<List<XrayHistoryDto>> GetHistoryAsync();
    }
}
