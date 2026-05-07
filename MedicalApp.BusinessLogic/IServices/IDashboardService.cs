using MedicalApp.BusinessLogic.DTOs.DashboardDtos;

namespace MedicalApp.BusinessLogic.IServices
{
    public interface IDashboardService
    {
        public Task<DashboardDto> GetDashboardAsync();
        public Task<DashboardDto>GetDashboardByDoctorAsync(string userId);
    }
}
