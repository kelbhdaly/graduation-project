using System.Text.Json;
namespace MedicalApp.BusinessLogic.Services
{
    public class XrayAiClient : IXrayAiClient
    {
        private readonly HttpClient _httpClient;

        public XrayAiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<XrayApiResult> PredictAsync(IFormFile file)
        {
            using var content = new MultipartFormDataContent();

            using var stream = file.OpenReadStream();
            content.Add(new StreamContent(stream), "file", file.FileName);

            var response = await _httpClient.PostAsync("/predict", content);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<XrayApiResult>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
