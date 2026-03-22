namespace MedicalApp.DataAccess.HandleExceptions
{
    public class InvalidEmailException(string message) : Exception(message)
    {
    }
}
