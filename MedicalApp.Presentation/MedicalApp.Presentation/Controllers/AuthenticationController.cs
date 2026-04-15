namespace MedicalApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("register/doctor")]
        public async Task<IActionResult> DoctorRegister(DoctorRegisterDto doctorRegisterDto)
        {

            var result = await _authenticationService.DoctorRegisterAsync(doctorRegisterDto);
            return Ok(result);
        }

        [HttpPost("register/patient")]

        public async Task<IActionResult> PatientRegister(PatientDto patientDto)
        {
            var result = await _authenticationService.PatientRegisterAsync(patientDto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authenticationService.LoginAsync(loginDto);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var result = await _authenticationService.GetMeAsync();
            return Ok(result);
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(SendOtpDto sendOtpDto)
        {
            var result = await _authenticationService.SendOtpAsync(sendOtpDto);
            return Ok(result);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto verifyOtpDto)
        {
            var result = await _authenticationService.VerifyOtpAsync(verifyOtpDto);
            return Ok(result);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            var result = await _authenticationService.ForgetPasswordAsync(forgetPasswordDto);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await _authenticationService.ResetPasswordAsync(resetPasswordDto);
            return Ok(result);

        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var result = await _authenticationService.RefreshTokenAsync(refreshTokenRequestDto);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequestDto refreshTokenRequestDto )
        {
            await _authenticationService.LogoutAsync(refreshTokenRequestDto);
            return Ok("Logged out successfully");
        }
    }
}
