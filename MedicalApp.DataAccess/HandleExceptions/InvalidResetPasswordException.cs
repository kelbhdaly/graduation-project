namespace MedicalApp.DataAccess.HandleExceptions
{
    public class InvalidResetPasswordException(string message) : Exception(message)
    {

    }
}
