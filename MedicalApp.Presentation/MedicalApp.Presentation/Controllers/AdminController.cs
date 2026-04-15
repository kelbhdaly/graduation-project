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


        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("pending-users")]
        public async Task<IActionResult> GetAllUserPanding()
        {
            var users = await _userManagementService.GetAllUserPandingAsync();
            return Ok(users);
        }

        [HttpPut("approve/{userId}")]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            var result = await _userManagementService.ApproveUserAsync(userId);
            return Ok(result);
        }
        [HttpPut("reject/{userId}")]
        public async Task<IActionResult> RejectUser(string userId)
        {
            var result = await _userManagementService.RejectUserAsync(userId);
            return Ok(result);
        }

        [HttpPut("disable/{userId}")]
        public async Task<IActionResult> DisableUser(string userId)
        {
            var result = await _userManagementService.DisableUserAsync(userId);
            return Ok(result);
        }

        [HttpPut("enable/{userId}")]
        public async Task<IActionResult> EnableUser(string userId)
        {
            var result = await _userManagementService.EnableUserAsync(userId);
            return Ok(result);
        }
        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userManagementService.DeleteUserAsync(userId);
            return Ok(result);
        }

        [HttpPut("restore/{userId}")]
        public async Task<IActionResult> RestoreUser(string userId)
        {
            var result = await _userManagementService.RestoreUserAsync(userId);
            return Ok(result);
        }

        [HttpGet("pending-doctors")]
        public async Task<IActionResult> GetAllDoctorPanding()
        {
            var users = await _userManagementService.GetAllDoctorPanding();
            return Ok(users);
        }

        [HttpGet("pending-patients")]
        public async Task<IActionResult> GetAllPatientPanding()
        {
            var users = await _userManagementService.GetAllPatientPanding();
            return Ok(users);

        }
    }
}
