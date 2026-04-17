namespace MedicalApp.DataAccess.HandleExceptions
{
    public class FailedUpdateUserException(string message) : Exception(message)
    {
    }
}
