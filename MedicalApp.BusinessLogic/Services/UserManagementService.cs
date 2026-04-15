using System.Data;

namespace MedicalApp.BusinessLogic.Services
{
    internal class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _mailService;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserManagementService(UserManager<ApplicationUser> userManager, IMailService mailService, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _mailService = mailService;
            _contextAccessor = contextAccessor;
        }

        public async Task<List<GetUsersDTo>> GetAllUsersAsync()
        {
            var adminId = GetAdminId();
            var users = await _userManager.Users.Where(u => u.Id != adminId).ToListAsync();

            var result = new List<GetUsersDTo>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);

                result.Add(new GetUsersDTo
                {
                    UserId = u.Id,
                    Email = u.Email!,
                    UserName = u.UserName!,
                    Status = u.UserStatus.ToString(),
                    Role = roles.FirstOrDefault() ?? "No Role",
                    EmailStatus = u.EmailConfirmed ? "Confirmed" : "Unconfirmed",
                    CreatedAt = u.CreatedAt,
                    IsDeleted = u.IsDeleted
                });
            }
            return result.ToList();
        }


        public async Task<List<UsersPandingDto>> GetAllUserPandingAsync()
        {
            var users = await _userManager.Users.
                 Where(u => u.UserStatus == UserStatus.Pending).ToListAsync();


            if (users == null || !users.Any())
            {
                return new List<UsersPandingDto>();
            }

            List<UsersPandingDto> result = await UserPanding(users);
            return result.ToList();
        }

 
        public async Task<List<UsersPandingDto>> GetAllDoctorPanding()
        {
            var users = await _userManager.Users.
                Where(u => u.UserStatus == UserStatus.Pending && u.Doctor != null).ToListAsync();

            if (users == null || !users.Any())
            {
                return new List<UsersPandingDto>();
            }

            List<UsersPandingDto> result = await UserPanding(users);
            return result.ToList();
        }

        public async Task<List<UsersPandingDto>> GetAllPatientPanding()
        {
            var users = await _userManager.Users.
                Where(u => u.UserStatus == UserStatus.Pending && u.Patient != null).ToListAsync();

            if (users == null || !users.Any())
            {
                return new List<UsersPandingDto>();
            }

            var result = await UserPanding(users);

            return result.ToList();
        }




        public async Task<string> ApproveUserAsync(string userId)
        {
            var user = await GetUserAsync(userId);

            if (user.UserStatus != UserStatus.Pending)
                throw new BadRequestException("User is not pending");

            user.UserStatus = UserStatus.Active;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception("Failed to update user");

            await _mailService.SendEmailAsync(new EmailMessage
            {
                To = user.Email!,
                Subject = "Account Approved",
                Body = "Your account has been approved. You can now log in."
            });

            return "User approved successfully";
        }

        public async Task<string> RejectUserAsync(string userId)
        {
            var user = await GetUserAsync(userId);

            user.UserStatus = UserStatus.Rejected;
            await _userManager.UpdateAsync(user);

            EmailMessage emailMessage = new EmailMessage
            {
                To = user.Email!,
                Subject = "Account Rejected",
                Body = "Your account has been rejected. Please contact support for more information."
            };
            await _mailService.SendEmailAsync(emailMessage);
            return "User rejected successfully";
        }

        public async Task<string> DisableUserAsync(string userId)
        {
            var user = await GetUserAsync(userId);
            user.LockoutEnd = DateTimeOffset.MaxValue;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to disable user");

            await _mailService.SendEmailAsync(new EmailMessage
            {
                To = user.Email!,
                Subject = "Account Disabled",
                Body = "Your account has been disabled. Please contact support for more information."
            });
            return "User disabled successfully";
        }
        public async Task<string> EnableUserAsync(string userId)
        {
            var user = await GetUserAsync(userId);

            user.LockoutEnd = null;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new EnableUserException("Failed to enable user");

            await _mailService.SendEmailAsync(new EmailMessage
            {
                To = user.Email!,
                Subject = "Account Enabled",
                Body = "Your account has been enabled. You can now log in."
            });

            return "User enabled successfully";
        }

        public async Task<string> DeleteUserAsync(string userId)
        {
            var user = await ToggleDeleteAsync(userId, true);

            await _mailService.SendEmailAsync(new EmailMessage
            {
                To = user.Email!,
                Subject = "Account Deleted",
                Body = "Your account has been deleted. Please contact support for more information."
            });
            return "User deleted successfully";
        }


        public async Task<string> RestoreUserAsync(string userId)
        {
            var user = await ToggleDeleteAsync(userId, false);
            await _mailService.SendEmailAsync(new EmailMessage
            {
                To = user.Email!,
                Subject = "Account Restored",
                Body = "Your account has been restored. You can now log in."
            });
            return "User restored successfully";
        }



        #region ExtractMethod

        private async Task<ApplicationUser> GetUserAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new NotFoundUserException("User not found");
            return user;
        }

        private async Task<ApplicationUser> ToggleDeleteAsync(string userId, bool isDeleted)
        {
            var user = await GetUserAsync(userId);
            user.IsDeleted = isDeleted;
            if (user.IsDeleted == isDeleted)
            {
                throw new BadRequestException($"User already in this IsDelete = {isDeleted}");
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new ToggleDeleteUserException("Failed to delete user");
            return user;
        }
        private string GetAdminId()
        {
            var userId = _contextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not authenticated");

            return userId;
        }


        private async Task<List<UsersPandingDto>> UserPanding(List<ApplicationUser> users)
        {
            var result = new List<UsersPandingDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UsersPandingDto
                {
                    UserId = user.Id,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    Status = user.UserStatus.ToString(),
                    Role = roles.FirstOrDefault() ?? "No Role",
                    EmailStatus = user.EmailConfirmed ? "Confirmed" : "Unconfirmed",
                    CreatedAt = user.CreatedAt,
                });

            }

            return result;
        }


        #endregion
    }
}
