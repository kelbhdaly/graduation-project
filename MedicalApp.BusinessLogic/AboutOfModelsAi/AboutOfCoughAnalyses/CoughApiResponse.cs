using System.Text.Json.Serialization;

namespace MedicalApp.BusinessLogic.AboutOfModelsAi.AboutOfCoughAnalyses
{
    public class CoughApiResponse
    {
        [JsonPropertyName("support_label")]
        public string SupportLabel { get; set; }

        [JsonPropertyName("risk_score")]
        public double RiskScore { get; set; }

        [JsonPropertyName("class_probabilities")]
        public Dictionary<string, double> ClassProbabilities { get; set; }

        [JsonPropertyName("clinical_use")]
        public string ClinicalUse { get; set; }

        [JsonPropertyName("disclaimer")]
        public string Disclaimer { get; set; }
    }
}
