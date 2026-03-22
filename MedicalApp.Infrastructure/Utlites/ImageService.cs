namespace MedicalApp.Infrastructure.Utlites
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public Task DeleteImageAsync(string imagePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        public async Task<string> UpdateImageAsync(IFormFile newImage, string oldImagePath, string folderName)
        {
            await DeleteImageAsync(oldImagePath);

            var images = new List<IFormFile> { newImage };

            var result = await UploadImagesAsync(images, folderName);

            return result.First();
        }

        public async Task<List<string>> UploadImagesAsync(List<IFormFile> images, string folderName)
        {
            var imageUrls = new List<string>();

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var folderPath = Path.Combine(_environment.WebRootPath, "images", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            foreach (var image in images)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    throw new Exception("Invalid image format");

                var fileName = Guid.NewGuid() + extension;

                var path = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imageUrls.Add($"/images/{folderName}/{fileName}");
            }

            return imageUrls;
        }

    }
}
