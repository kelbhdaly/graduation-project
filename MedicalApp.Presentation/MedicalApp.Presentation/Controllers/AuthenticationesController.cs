namespace MedicalApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationesController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationesController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("DoctorRegister")]
        public async Task<IActionResult> DoctorRegister(DoctorRegisterDto doctorRegisterDto)
        {

            var result = await _authenticationService.DoctorRegisterAsync(doctorRegisterDto);
            return Ok(result);
        }
        [HttpPost("PatientRegister")]
        public async Task<IActionResult> PatientRegister(PatientDto patientDto)
        {
            var result = await _authenticationService.PatientRegisterAsync(patientDto);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authenticationService.LoginAsync(loginDto);
            return Ok(result);
        }


        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailDTO confirmEmailDTO)
        {
            var result = await _authenticationService.ConfirmEmailAsync(confirmEmailDTO);
            return Ok(result);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            var result = await _authenticationService.ForgetPasswordAsync(forgetPasswordDto);
            return Ok(result);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await _authenticationService.ResetPasswordAsync(resetPasswordDto);
            return Ok(result);

        }
    }
}
