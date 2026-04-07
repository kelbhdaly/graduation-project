namespace MedicalApp.BusinessLogic.Services
{
    internal class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _mailService;

        public UserManagementService(UserManager<ApplicationUser> userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }
        public async Task<List<UsersPandingDto>> GetAllUserPandingAsync()
        {
            var users = await _userManager.Users.
                 Where(u => u.UserStatus == UserStatus.Pending).ToListAsync();
            if (users == null || !users.Any())
            {
                return new List<UsersPandingDto>();
            }
            return users.Select(u => new UsersPandingDto
            {
                UserId = u.Id,
                Email = u.Email!,
                UserName = u.UserName!,
                Status = u.UserStatus.ToString()


            }).ToList();

        }

        public async Task<string> ApproveUserAsync(string userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundUserException("User not found");

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
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new NotFoundUserException("User not found");

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
   
    
    }
}
