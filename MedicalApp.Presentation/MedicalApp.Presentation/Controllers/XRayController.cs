using MedicalApp.BusinessLogic.DTOs.AiDtos;

namespace MedicalApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XRayController : ControllerBase
    {
        private readonly IXrayService _xrayService;
        public XRayController(IXrayService xrayService)
        {
            _xrayService = xrayService;
        }
        [HttpPost("xray")]
        public async Task<IActionResult> AnalyzeXray([FromForm] XrayRequestDto dto)
        {
            var result = await _xrayService.AnalyzeAsync(dto.Image);
            return Ok(result);
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var result = await _xrayService.GetHistoryAsync();
            return Ok(result);
        }
    }
}