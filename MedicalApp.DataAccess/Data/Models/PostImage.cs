namespace MedicalApp.DataAccess.Data.Models
{
    public class PostImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = default!;

        public int PostId { get; set; }

        public Post Post { get; set; } = default!;
    }
}
