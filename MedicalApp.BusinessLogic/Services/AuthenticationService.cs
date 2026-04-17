using Microsoft.AspNetCore.Http;

namespace MedicalApp.BusinessLogic.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration, IMapper mapper, IMailService mailService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> DoctorRegisterAsync(DoctorRegisterDto doctorRegisterDto)
        {
            ApplicationUser user = await CreateUserAsync(doctorRegisterDto);

            await _userManager.AddToRoleAsync(user, "Doctor");

            var doctor = _mapper.Map<Doctor>(doctorRegisterDto);

            doctor.UserId = user.Id;

            _dbContext.Doctors.Add(doctor);
            await _dbContext.SaveChangesAsync();

            await SendOtpAsync(new SendOtpDto
            {
                Email = user.Email!
            });
            return "Doctor Registered Successfully. Check your email.";
        }

        public async Task<string> PatientRegisterAsync(RegisterDto registerDto)
        {
            ApplicationUser user = await CreateUserAsync(registerDto);

            await _userManager.AddToRoleAsync(user, "Patient");

            var patient = _mapper.Map<Patient>(registerDto);

            patient.UserId = user.Id;

            _dbContext.Patients.Add(patient);
            await _dbContext.SaveChangesAsync();

            await SendOtpAsync(new SendOtpDto
            {
                Email = user.Email!
            });
            return "Patient  Registered Successfully. Check your email. ";
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            //Check Email 
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            if (user.IsDeleted)
                throw new UnauthorizedAccessException("This account has been deleted");

            if (!user.EmailConfirmed)
                throw new UnauthorizedAccessException("Please verify your email first");

            if (user.UserStatus == UserStatus.Rejected)
                throw new UnauthorizedAccessException("Your Account Is Rejected");

         
            if (user.UserStatus == UserStatus.Disabled)
                throw new UnauthorizedAccessException("Your account is disabled");

            if (user.IsDeleted == true)
                throw new UnauthorizedAccessException("Your account is Deleted");



            if (user.UserStatus != UserStatus.Active)
                throw new UnauthorizedAccessException("Your account is not approved yet");


            //Check Password
            var passwordIsValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordIsValid)
            {
                throw new UnauthorizedAccessException("Invalid Eamil or Password");
            }
            var roles = await _userManager.GetRolesAsync(user);

            var refreshToken = GenerateRefreshToken();
            refreshToken.UserId = user.Id;

            user.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new UserDto
            {
                Email = user.Email!,
                UserName = user.UserName!,
                Role = roles.FirstOrDefault() ?? "",
                Token = await GenerateJwtToken(user),
                RefreshToken = refreshToken.Token
            };


        }

        public async Task<MeDto> GetMeAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");
            if (user.UserStatus != UserStatus.Active)
                throw new UnauthorizedAccessException("User not active");
            var roles = await _userManager.GetRolesAsync(user);
            return new MeDto
            {
                Id = user.Id,
                Name = user.UserName!,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? ""
            };
        }

        public async Task<string> SendOtpAsync(SendOtpDto sendOtpDto)
        {
            var user = await _userManager.FindByEmailAsync(sendOtpDto.Email);

            if (user == null)
                throw new Exception("User not found");

            _dbContext.OtpCodes.RemoveRange(
                _dbContext.OtpCodes.
                Where(x => x.Email == sendOtpDto.Email && !x.IsUsed || x.ExpireAt < DateTime.UtcNow));
            var code = GenerateOtp();

            var otp = new OtpCode
            {
                Email = user.Email!,
                Code = code,
                ExpireAt = DateTime.UtcNow.AddMinutes(5)
            };

            _dbContext.OtpCodes.Add(otp);
            await _dbContext.SaveChangesAsync();

            await _mailService.SendEmailAsync(new EmailMessage
            {
                To = user.Email!,
                Subject = "Your OTP Code",
                Body = $"Your OTP is: {code}"
            });

            return "OTP sent";
        }
        public async Task<string> VerifyOtpAsync(VerifyOtpDto verifyOtpDto)
        {
            var otp = await _dbContext.OtpCodes
                .FirstOrDefaultAsync(x =>
                    x.Email == verifyOtpDto.Email &&
                    x.Code == verifyOtpDto.Code &&
                    !x.IsUsed);

            if (otp == null || otp.ExpireAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired OTP");

            otp.IsUsed = true;

            var user = await _userManager.FindByEmailAsync(verifyOtpDto.Email);

            if (user != null)
            {
                user.EmailConfirmed = true;
            }

            await _dbContext.SaveChangesAsync();

            return "OTP verified successfully";
        }


        public async Task<string> ForgetPasswordAsync(ForgetPasswordDto forgetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);

            if (user == null || !user.EmailConfirmed)
            {
                return "If the email exists, a reset link has been sent.";
            }

            await SendOtpAsync(
                  new SendOtpDto
                  {
                      Email = user.Email!
                  });



            return "Reset code sent to your email.";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var otp = await _dbContext.OtpCodes.FirstOrDefaultAsync
        (x => x.Email == resetPasswordDto.Email && x.Code == resetPasswordDto.Code && !x.IsUsed);

            if (otp == null || otp.ExpireAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired OTP");
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                throw new NotFoundUserException("User not found");
            otp.IsUsed = true;
           
            var removePassword = await _userManager.RemovePasswordAsync(user);
            if (!removePassword.Succeeded)
                throw new Exception("Failed to reset password");

            var addNewPassword = await _userManager.AddPasswordAsync(user, resetPasswordDto.NewPassword);
            if (!addNewPassword.Succeeded)
                throw new InvalidResetPasswordException("Failed to set new password");

            await _dbContext.SaveChangesAsync();

            return "Password has been reset successfully";
        }


        public async Task<UserDto> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var refreshToken = await _dbContext.RefreshTokens
               .Include(x => x.User)
                  .FirstOrDefaultAsync(x => x.Token == refreshTokenRequestDto.Token);

            if (refreshToken == null || refreshToken.IsRevoked
                || refreshToken.ExpiresOn <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid refresh token");

            var user = refreshToken.User;
            refreshToken.IsRevoked = true;
            var newRefreshToken = GenerateRefreshToken();
            newRefreshToken.UserId = user.Id;

            user.RefreshTokens.Add(newRefreshToken);

            await _dbContext.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Email = user.Email!,
                UserName = user.UserName!,
                Role = roles.FirstOrDefault() ?? "",
                Token = await GenerateJwtToken(user),
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task LogoutAsync(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var refreshToken = await _dbContext.RefreshTokens
                     .FirstOrDefaultAsync(x => x.Token == refreshTokenRequestDto.Token);

            if (refreshToken == null)
                throw new UnauthorizedAccessException("Invalid token");

            refreshToken.IsRevoked = true;

            await _dbContext.SaveChangesAsync();
        }

        #region Extract Method
        private async Task<ApplicationUser> CreateUserAsync(CreateUserDto createUserDto)
        {
            var emailExist = await _userManager.FindByEmailAsync(createUserDto.Email);
            if (emailExist != null)
                throw new InvalidEmailException("Email already exists.");


            var user = new ApplicationUser
            {
                UserName = createUserDto.Email,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                UserStatus = UserStatus.Pending
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
                throw new InvalidCreateException("Can't Create User");
            return user;
        }



        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email , user.Email!),
            new Claim(ClaimTypes.Name , user.UserName!),
            new Claim(ClaimTypes.NameIdentifier , user.Id)
        };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

            }
            claims.Add(new Claim("UserType", roles.FirstOrDefault() ?? ""));
            var secKey = _configuration["JWT:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
        }

        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User
                       .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("User not authenticated");

            return userId;
        }


        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // 6 digits
        }
        #endregion
    }

}

