namespace MedicalApp.DataAccess.HandleExceptions
{
    public class DisableUserException : Exception
    {
        public DisableUserException(string message) : base(message)
        {
        }
    }
}
