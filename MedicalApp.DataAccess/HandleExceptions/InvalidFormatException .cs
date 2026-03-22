namespace MedicalApp.DataAccess.HandleExceptions
{
    public class InvalidFormatException(string message) : Exception(message)
    {
    }
}
