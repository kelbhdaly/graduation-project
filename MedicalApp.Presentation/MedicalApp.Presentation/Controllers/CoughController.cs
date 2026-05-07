namespace MedicalApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoughController : ControllerBase
    {
        private readonly ICoughService _coughService;
        public CoughController(ICoughService coughService)
        {
            _coughService = coughService;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze(IFormFile audio)
        {
            var result = await _coughService.AnalyzeAsync(audio);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var history = await _coughService.GetHistoryAsync();
            return Ok(history);
        }
    }
}
