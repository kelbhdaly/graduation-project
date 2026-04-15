namespace MedicalApp.BusinessLogic.IServices
{
    public interface IGenericAiClient
    {
        Task<TResponse> PostImageAsync<TResponse>(string url, IFormFile file);
        Task<TResponse> PostJsonAsync<TRequest, TResponse>(string url, TRequest data);
    }
}
