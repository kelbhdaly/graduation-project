using System.Text.Json.Serialization;

namespace MedicalApp.BusinessLogic.AboutOfModelsAi.AboutOfStethoscopeAnalysis
{
    public class StethoscopeApiResponse
    {
        [JsonPropertyName("predicted_class")]
        public string PredictedClass { get; set; }

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }
    }
}
