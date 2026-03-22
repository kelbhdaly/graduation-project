namespace MedicalApp.BusinessLogic.Services
{
    public class FavoritePostService : IFavoritePostService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FavoritePostService(ApplicationDbContext context, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<FavoritePost> AddToFavoriteAsync(AddToFavoriteDto favoritePostsDto)
        {
            var userId = GetUserId();

            var postExists = await _context.Posts.AnyAsync(p => p.Id == favoritePostsDto.PostId);

            if (!postExists)
                throw new PostNotFoundException("Post not found");

            var exists = await _context.FavoritePosts.
                 AnyAsync(F => F.UserId == userId && F.PostId == favoritePostsDto.PostId);

            if (exists)
                throw new PostAlreadyInFavoritesException("Post is already in favorites.");


            var favorite = new FavoritePost
            {
                UserId = userId,
                PostId = favoritePostsDto.PostId,
                
                
            };
            await _context.FavoritePosts.AddAsync(favorite);
            await _context.SaveChangesAsync();
            return favorite;
        }

        public async Task<List<PostDto>> GetFavoritePostsAsync()
        {
            var userId = GetUserId();

            var favoritePosts = await _context.FavoritePosts
                .Where(f => f.UserId == userId)
                .Include(f => f.Post)
                .ThenInclude(p => p.PostImages)
                .Select(f => f.Post)
                .ToListAsync();
            if (!favoritePosts.Any())
                throw new NotFoundFavoritePostException("No favorite posts found");

            var result = _mapper.Map<List<PostDto>>(favoritePosts);
            return result;
        }

        public async Task RemoveFromFavoriteAsync(AddToFavoriteDto favoritePostsDto)
        {
            var userId = GetUserId();

            var favorite = await _context.FavoritePosts
                       .FirstOrDefaultAsync(f => f.UserId == userId &&
                       f.PostId == favoritePostsDto.PostId);

            if (favorite == null)
                throw new NotFoundFavoritePostException("Favorite not found");

            _context.FavoritePosts.Remove(favorite);

            await _context.SaveChangesAsync();
        }






        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new UnauthorizedException("User not authenticated");

            return userId;
        }


    }

}

