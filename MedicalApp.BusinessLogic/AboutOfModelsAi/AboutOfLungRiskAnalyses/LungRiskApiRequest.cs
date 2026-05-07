using System.Text.Json.Serialization;

namespace MedicalApp.BusinessLogic.AboutOfModelsAi.AboutOfLungRiskAnalyses
{
    public class LungRiskApiRequest
    {
        [JsonPropertyName("features")]
        public FeaturesModel Features { get; set; }
    }
}
