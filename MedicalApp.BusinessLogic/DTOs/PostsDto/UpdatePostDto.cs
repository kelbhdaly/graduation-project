    namespace MedicalApp.BusinessLogic.DTOs.Posts
    {
        public class UpdatePostDto
        {
            public string Title { get; set; } = default!;

            public string Description { get; set; } = default!;

            public List<IFormFile>? Images { get; set; }
        }
    }
