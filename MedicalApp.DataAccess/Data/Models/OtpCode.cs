namespace MedicalApp.DataAccess.Data.Models
{
    public class OtpCode
    {
        public int Id { get; set; }

        public string Email { get; set; } =default!;

        public string Code { get; set; } = default!;

        public DateTime ExpireAt { get; set; }

        public bool IsUsed { get; set; } = false;
    }
}
