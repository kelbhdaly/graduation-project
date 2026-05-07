namespace MedicalApp.Infrastructure.Utlites
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
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

        public async Task<string> SaveBase64ImageAsync(string base64, string folderName)
        {
            var folderPath = Path.Combine(_environment.WebRootPath, "images", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + ".png";

            var path = Path.Combine(folderPath, fileName);

            var bytes = Convert.FromBase64String(base64);

            await File.WriteAllBytesAsync(path, bytes);

            return $"/images/{folderName}/{fileName}";
        }


        public async Task<List<string>> UploadAudioAsync(List<IFormFile> files, string folderName)
        {



            var audioUrls = new List<string>();

            var allowedExtensions = new[]
            { ".wav", ".mp3", ".m4a", ".webm", ".ogg", ".flac" };

            var folderPath = Path.Combine(_environment.WebRootPath, "audio", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                ValidateAudio(file);
                if (!allowedExtensions.Contains(extension))
                    throw new Exception("Invalid audio format");

                var fileName = Guid.NewGuid() + extension;

                var path = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                audioUrls.Add($"/audio/{folderName}/{fileName}");
            }

            return audioUrls;
        }



        #region Private Method

        private void ValidateAudio(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Audio file is required");

            var allowedExtensions = new[] { ".wav", ".mp3", ".ogg", ".m4a" };

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid audio format");

            if (file.Length > 10 * 1024 * 1024) // 10MB
                throw new Exception("File too large");
        }

        #endregion
    }
}
