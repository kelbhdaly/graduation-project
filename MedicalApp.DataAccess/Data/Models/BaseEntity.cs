namespace MedicalApp.DataAccess.Data.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Max Length Of First Name 50 Please Follow This Formate")]
        public string FirstName { get; set; } = default!;
        [Required, MaxLength(50, ErrorMessage = "Max Length Of First Name 50 Please Follow This Formate")]
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public string NationalId { get; set; } = default!;
        public Gender Gender { get; set; }
        public IsMarried IsMarried { get; set; }
        public Governorate Governorate { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        // Navigation property 
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
    }
}
