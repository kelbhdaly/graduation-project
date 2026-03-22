namespace MedicalApp.DataAccess.Data.Models
{
    public class Doctor : BaseEntity
    {

        public string MedicalLicense { get; set; } = default!;

        public List<Post> Posts { get; set; } = new List<Post>();

    }
}
