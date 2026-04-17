namespace MedicalApp.BusinessLogic.IServices
{
    public interface IUserManagementService
    {
        Task<List<GetUsersDTo>> GetAllUsersAsync();
        Task<List<UsersPendingDto>> GetAllUserPendingAsync();
        Task<List<UsersPendingDto>> GetAllDoctorPending();
        Task<List<UsersPendingDto>> GetAllPatientPending();
        Task<string> ApproveUserAsync(string userId);
        Task<string> RejectUserAsync(string userId);
        Task<string> DisableUserAsync(string userId);        
        Task<string> EnableUserAsync(string userId);
        Task<string> DeleteUserAsync(string userId);
        Task<string> RestoreUserAsync(string userId);


    }
}
