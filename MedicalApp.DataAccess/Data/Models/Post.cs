namespace MedicalApp.DataAccess.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required , MaxLength(60)]
        public string Title { get; set; } = default!;
        [Required , MaxLength(300)]
        public string Description { get; set; } = default!;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = default!;
        public List<PostImage> PostImages { get; set; } = new List<PostImage>();
    }
}
