using MedicalApp.Infrastructure.RolesInApplication;

namespace MedicalApp.BusinessLogic.Services
{
    public class DataSeeding : IDataSeeding
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public DataSeeding(RoleManager<IdentityRole> roleManager,
                UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task IdentityDataSeedAsync()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
        }




        private async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync(AppRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));

            if (!await _roleManager.RoleExistsAsync(AppRoles.Doctor))
                await _roleManager.CreateAsync(new IdentityRole(AppRoles.Doctor));

            if (!await _roleManager.RoleExistsAsync(AppRoles.Patient))
                await _roleManager.CreateAsync(new IdentityRole(AppRoles.Patient));
        }

        private async Task SeedUsersAsync()
        {
            
            {
                await CreateUserAsync(
                    email: "khaled@gmail.com",
                    username: "KhaledMohamed",
                    role: AppRoles.Admin);

                await CreateUserAsync(
                    email: "Ali@gmail.com",
                    username: "AliMohamed",
                    role: AppRoles.Patient);

                await CreateUserAsync(
                    email: "khaled123@gmail.com",
                    username: "khaled",
                    role: AppRoles.Doctor);
            }
        }

        private async Task CreateUserAsync(string email, string username, string role)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return;

            var user = new ApplicationUser
            {
                Email = email,
                UserName = username,
                PhoneNumber = "1234567890",
                UserStatus = UserStatus.Active
            };
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, "P@ssw0rd");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }
   
    
    }
}
