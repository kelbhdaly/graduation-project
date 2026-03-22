namespace MedicalApp.DataAccess.HandleExceptions
{
    public class InvalidPasswordException(string message) : Exception(message)
    {
    }
}
