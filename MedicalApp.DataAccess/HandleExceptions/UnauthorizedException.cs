namespace MedicalApp.DataAccess.HandleExceptions
{
    public class UnauthorizedException(string message) : Exception(message)
    {
    }
}
