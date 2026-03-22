namespace MedicalApp.DataAccess.HandleExceptions
{
    public class PostAlreadyInFavoritesException(string message) : Exception(message)
    {
    }
}
