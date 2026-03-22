namespace MedicalApp.BusinessLogic.IServices
{
    public interface IFavoritePostService
    {
        Task<FavoritePost> AddToFavoriteAsync(AddToFavoriteDto favoritePostsDto);

        Task RemoveFromFavoriteAsync(AddToFavoriteDto favoritePostsDto);

        Task<List<PostDto>> GetFavoritePostsAsync();
    }
}
