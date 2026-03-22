using MedicalApp.BusinessLogic.DTOs.FavoritePostsDto;

namespace MedicalApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoritePostService _favoriteService;

        public FavoriteController(IFavoritePostService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites([FromBody] AddToFavoriteDto addToFavoriteDto)
        {
            var fovarite = await _favoriteService.AddToFavoriteAsync(addToFavoriteDto);
            return Ok(fovarite);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] AddToFavoriteDto addToFavoriteDto)
        {
            await _favoriteService.RemoveFromFavoriteAsync(addToFavoriteDto);
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetFavoritePosts()
        {
            var favoritePosts = await _favoriteService.GetFavoritePostsAsync();
            return Ok(favoritePosts);

        }
    }
}
