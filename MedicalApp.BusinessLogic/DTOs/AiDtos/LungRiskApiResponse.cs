using System.Text.Json.Serialization;

namespace MedicalApp.BusinessLogic.DTOs.AiDtos
{
    public class LungRiskApiResponse
    {
        public int Predicted_Class_Index { get; set; }
        public string Predicted_Class_Name { get; set; } =string.Empty;
        [JsonPropertyName("probabilities")]
        public Dictionary<string, double> Probabilities { get; set; }
    }
}
