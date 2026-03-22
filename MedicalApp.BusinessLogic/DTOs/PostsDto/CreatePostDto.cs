namespace MedicalApp.BusinessLogic.DTOs.Posts
{
    public class CreatePostDto
    {
        public string Title { get; set; } = default!;

        public string Description { get; set; } = default!;

        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
