namespace MedicalApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StethoscopeController : ControllerBase
    {
        private readonly IStethoscopeService _stethoscopeService;
        public StethoscopeController(IStethoscopeService stethoscopeService)
        {
            _stethoscopeService = stethoscopeService;
        }

        [Authorize(Roles = "DOCTOR")]
        [HttpPost("analyze")]

        public async Task<ActionResult<StethoscopeResponseDto>> AnalyzeAsync([FromForm] StethoscopeRequestDto stethoscopeRequestDto)
        {
            var result = await _stethoscopeService.AnalyzeAsync(stethoscopeRequestDto);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<ActionResult<List<HistoryStethoscopeAnalysisDto>>> GetAnalysisByPatientIdAsync()
        {
            var result = await _stethoscopeService.GetAnalysisByPatientIdAsync();
            return Ok(result);
        }
    }
}
