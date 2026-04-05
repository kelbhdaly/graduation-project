namespace MedicalApp.BusinessLogic.IServices
{
    public interface IAiClient
    {
        Task<AiResultDto> ProcessImageAsync(IFormFile image);
    }
}
