namespace MedicalApp.BusinessLogic.IServices
{
    public interface IUserManagementService
    {
        Task<List<GetUsersDTo>> GetAllUsersAsync();
        Task<List<UsersPandingDto>> GetAllUserPandingAsync();
        Task<List<UsersPandingDto>> GetAllDoctorPanding();
        Task<List<UsersPandingDto>> GetAllPatientPanding();
        Task<string> ApproveUserAsync(string userId);
        Task<string> RejectUserAsync(string userId);
        Task<string> DisableUserAsync(string userId);        
        Task<string> EnableUserAsync(string userId);
        Task<string> DeleteUserAsync(string userId);
        Task<string> RestoreUserAsync(string userId);


    }
}
