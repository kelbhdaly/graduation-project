using MedicalApp.BusinessLogic.Ai;
using System.Text.Json.Serialization;

namespace MedicalApp.BusinessLogic.DTOs.AiDtos
{
    public class LungRiskApiRequest
    {
        [JsonPropertyName("features")]
        public FeaturesModel Features { get; set; }
    }
}
