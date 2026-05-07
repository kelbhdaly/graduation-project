
namespace MedicalApp.BusinessLogic.IServices
{
    public interface IFileService
    {
        Task<List<string>> UploadImagesAsync(List<IFormFile> images, string folderName);
        Task DeleteImageAsync(string imagePath);
        Task<string> UpdateImageAsync(IFormFile newImage, string oldImagePath, string folderName);
        public Task<string> SaveBase64ImageAsync(string base64, string folderName);

        public Task<List<string>> UploadAudioAsync(List<IFormFile> files, string folderName);
    }
}
