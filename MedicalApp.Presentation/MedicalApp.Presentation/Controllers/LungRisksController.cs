using MedicalApp.BusinessLogic.DTOs.AiDtos;

namespace MedicalApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LungRisksController : ControllerBase
    {

        private readonly ILungRiskService _lungRiskService;
        public LungRisksController(ILungRiskService lungRiskService)
        {
            _lungRiskService = lungRiskService;
        }



        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromBody] LungRiskRequestDto dto)
        {
            var result = await _lungRiskService.AnalyzeAsync(dto);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var result = await _lungRiskService.GetHistoryAsync();
            return Ok(result);
        }
    }
}
