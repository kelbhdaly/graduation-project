namespace MedicalApp.BusinessLogic.IServices
{
    public interface IUserManagementService
    {
        Task<List<UsersPandingDto>> GetAllUserPandingAsync();
        Task<string> ApproveUserAsync(string userId);
        Task<string> RejectUserAsync(string userId);
    }
}
