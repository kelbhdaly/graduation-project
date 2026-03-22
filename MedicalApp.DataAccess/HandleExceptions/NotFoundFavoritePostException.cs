namespace MedicalApp.DataAccess.HandleExceptions
{
    public class NotFoundFavoritePostException(string message) : Exception(message)
    {
    }
}
