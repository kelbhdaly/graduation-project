namespace MedicalApp.DataAccess.HandleExceptions
{
    public class ToggleDeleteUserException(string message) : Exception(message)
    {
    }
}
