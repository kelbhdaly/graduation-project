namespace MedicalApp.BusinessLogic.DTOs.Posts
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string DoctorName { get; set; } = default!;
        public List<PostImageDto> ImageUrls { get; set; } = new List<PostImageDto>();
    }
}
