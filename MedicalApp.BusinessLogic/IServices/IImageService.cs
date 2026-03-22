
namespace MedicalApp.BusinessLogic.IServices
{
    public interface IImageService
    {
        Task<List<string>> UploadImagesAsync(List<IFormFile> images, string folderName);
        Task DeleteImageAsync(string imagePath);
        Task<string> UpdateImageAsync(IFormFile newImage, string oldImagePath, string folderName);
    }
}
