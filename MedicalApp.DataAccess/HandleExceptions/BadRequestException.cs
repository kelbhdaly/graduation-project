namespace MedicalApp.DataAccess.HandleExceptions
{
    public class BadRequestException(string message) : Exception(message)
    {
    }
}
