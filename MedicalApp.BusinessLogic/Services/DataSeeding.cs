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
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Doctor"));
                    await _roleManager.CreateAsync(new IdentityRole("Patient"));
                }

                if (!_userManager.Users.Any())
                {
                    var doctor = new ApplicationUser()
                    {
                        Email = "khaled@gmail.com",
                        PhoneNumber = "1234567890",
                        UserName = "KhaledMohamed"
                    };
                    var patient = new ApplicationUser()
                    {
                        Email = "Ali@gmail.com",
                        PhoneNumber = "1234567890",
                        UserName = "AliMohamed"
                    };
                    await _userManager.CreateAsync(patient, "P@ssw0rd");
                    await _userManager.CreateAsync(doctor, "P@ssw0rd");
                    await _userManager.AddToRoleAsync(doctor, "Doctor");
                    await _userManager.AddToRoleAsync(patient, "Patient");

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
