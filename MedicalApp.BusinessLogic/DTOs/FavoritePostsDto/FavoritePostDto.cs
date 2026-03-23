namespace MedicalApp.BusinessLogic.DTOs.FavoritePostsDto
{
    public class FavoritePostDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Title { get; set; } = default!;
        public string DoctorName { get; set; } = default!;
        public List<string> Images { get; set; } = new List<string>();

    }
}
