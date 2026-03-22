namespace MedicalApp.DataAccess.HandleExceptions
{
    public class PostNotFoundException(string message) : Exception(message)
    {
    }
}
