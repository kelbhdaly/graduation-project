namespace MedicalApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboardData = await _dashboardService.GetDashboardAsync();
            return Ok(dashboardData);
        }


        [HttpPost("{userId}")]
        [Authorize(Roles = "DOCTOR")]
        public async Task<IActionResult> GetDashboardByDoctor( string userId)
        {
            var dashboardData = await _dashboardService.GetDashboardByDoctorAsync(userId);
            return Ok(dashboardData);
        }
    }
}
