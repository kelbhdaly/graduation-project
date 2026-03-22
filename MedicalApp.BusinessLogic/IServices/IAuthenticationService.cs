namespace MedicalApp.BusinessLogic.IServices
{
    public interface IAuthenticationService
    {
        Task<string> DoctorRegisterAsync(DoctorRegisterDto doctorRegisterDto);
        Task<string> PatientRegisterAsync(RegisterDto registerDto);
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<string> ConfirmEmailAsync(ConfirmEmailDTO confirmEmailDTO);
        Task<string> ForgetPasswordAsync(ForgetPasswordDto forgetPasswordDto);
        Task<string> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

    }
}
