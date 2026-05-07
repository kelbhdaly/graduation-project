namespace MedicalApp.BusinessLogic.IServices
{
    public interface IStethoscopeService
    {
        public Task<StethoscopeResponseDto> AnalyzeAsync(StethoscopeRequestDto stethoscopeRequestDto);
        public Task<List<HistoryStethoscopeAnalysisDto>> GetAnalysisByPatientIdAsync();
    }
}
