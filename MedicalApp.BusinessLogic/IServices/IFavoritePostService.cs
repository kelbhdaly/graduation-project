namespace MedicalApp.BusinessLogic.IServices
{
    public interface IFavoritePostService
    {
        Task<FavoritePostDto> AddToFavoriteAsync(AddToFavoriteDto favoritePostsDto);

        Task<List<PostDto>> GetFavoritePostsAsync();
        Task<string> RemoveFromFavoriteAsync(AddToFavoriteDto favoritePostsDto);

    }
}
