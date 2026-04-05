namespace MedicalApp.DataAccess.HandleExceptions
{
    public class NotFoundUserException(string message) : Exception(message)
    {
    }
}
