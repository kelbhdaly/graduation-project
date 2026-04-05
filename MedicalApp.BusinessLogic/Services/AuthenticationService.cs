namespace MedicalApp.BusinessLogic.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public AuthenticationService(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration, IMapper mapper, IMailService mailService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _mailService = mailService;
        }

        public async Task<string> DoctorRegisterAsync(DoctorRegisterDto doctorRegisterDto)
        {
            ApplicationUser user = await CreateUserAsync(doctorRegisterDto);

            await _userManager.AddToRoleAsync(user, "Doctor");

            var doctor = _mapper.Map<Doctor>(doctorRegisterDto);

            doctor.UserId = user.Id;

            _dbContext.Doctors.Add(doctor);
            await _dbContext.SaveChangesAsync();

            await SendConfirmationEmailAsync(user);
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

            await SendConfirmationEmailAsync(user);
            return "Patient  Registered Successfully. Check your email. ";
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            //Check Email 
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || user.UserStatus == UserStatus.Pending)
            {
                throw new UnauthorizedAccessException("Can't Login Your Acount Not Active Yet");
            }

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

        public async Task<string> ConfirmEmailAsync(ConfirmEmailDTO confirmEmailDto)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);

            if (user == null)
            {
                throw new InvalidEmailException("Invalid email or token");
            }

            if (user.EmailConfirmed)
            {
                throw new InvalidEmailException("Email already confirmed");
            }

            var decodedToken = Uri.UnescapeDataString(confirmEmailDto.Token);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
            {
                throw new InvalidEmailException("Invalid confirmation token");
            }

            return "Email Confirmed Successfully";
        }

        public async Task<string> ForgetPasswordAsync(ForgetPasswordDto forgetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);

            if (user == null || !user.EmailConfirmed)
            {
                return "If the email exists, a reset link has been sent.";
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink =
                $"{_configuration["FrontEnd:URL"]}/reset-password?email={user.Email}&token={Uri.EscapeDataString(token)}";

            var emailMessage = new EmailMessage
            {
                To = user.Email!,
                Subject = "Password Reset",
                Body = $@"
            <h2>Password Reset</h2>
            <p>Click the link below to reset your password:</p>
            <a href='{resetLink}'>Reset Password</a>"
            };

            await _mailService.SendEmailAsync(emailMessage);

            return " Link has been sent.";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordDto.Email) ||
                string.IsNullOrWhiteSpace(resetPasswordDto.Token) ||
                string.IsNullOrWhiteSpace(resetPasswordDto.NewPassword))
            {
                throw new BadRequestException("Invalid data");
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user == null || !user.EmailConfirmed)
            {
                return "Invalid email or token";
            }

            var token = Uri.UnescapeDataString(resetPasswordDto.Token);

            var resetResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);

            if (!resetResult.Succeeded)
            {
                return "Invalid email or token";
            }

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

        private async Task SendConfirmationEmailAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"https://localhost:5001/api/auth/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
            await _mailService.SendEmailAsync(new EmailMessage
            {

                To = user.Email!,
                Subject = "Confirm your email",
                Body = confirmationLink
            });
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

       
       

        #endregion
    }

}

