using MedicalApp.BusinessLogic.AboutOfModelsAi.AboutOfXrayAnalysis;

namespace MedicalApp.BusinessLogic.IServices
{
    public interface IXrayAiClient
    {
        Task<XrayApiResult> PredictAsync(IFormFile file);
    }
}
