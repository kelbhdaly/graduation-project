namespace MedicalApp.Presentation.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public AdminController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUserPanding()
        {
            var users = await _userManagementService.GetAllUserPandingAsync();
            return Ok(users);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            var result = await _userManagementService.ApproveUserAsync(userId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> RejectUser(string userId)
        {
            var result = await _userManagementService.RejectUserAsync(userId);
            return Ok(result);
        }
    }
}
