namespace MedicalApp.DataAccess.HandleExceptions
{
    public class InvalidRestPasswordException(string message): Exception(message)
    {
    }
}
