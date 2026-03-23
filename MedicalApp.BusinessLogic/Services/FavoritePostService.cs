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

        public async Task<FavoritePostDto> AddToFavoriteAsync(AddToFavoriteDto favoritePostsDto)
        {
            var userId = GetUserId();

            var post = await _context.Posts
                         .Include(p => p.Doctor)
                         .ThenInclude(d => d.ApplicationUser)
                         .Include(p => p.PostImages)
                         .FirstOrDefaultAsync(p => p.Id == favoritePostsDto.PostId);
            if (post == null)
                throw new PostNotFoundException("Post not found");

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
                Post = post
            };

            await _context.FavoritePosts.AddAsync(favorite);
            await _context.SaveChangesAsync();
            return new FavoritePostDto
            {
                Id = favorite.Id,
                PostId = post.Id,
                Title = post.Title,
                DoctorName = post.Doctor.ApplicationUser.UserName!,
                Images = post.PostImages.Select(i => i.ImageUrl).ToList()
            };
        }

        public async Task<List<PostDto>> GetFavoritePostsAsync()
        {
            var userId = GetUserId();
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            var favoritePosts = await _context.FavoritePosts.Where(f => f.UserId == userId)
                     .Select(f => new PostDto
                     {
                         Id = f.Post.Id,
                         Title = f.Post.Title,
                         Description = f.Post.Description,
                         DoctorName = f.Post.Doctor.ApplicationUser.UserName!,
                         ImageUrls = f.Post.PostImages.Select(i => new PostImageDto
                         {
                             ImageUrl = baseUrl + i.ImageUrl
                         }).ToList()
                     }).ToListAsync();
            if (!favoritePosts.Any())
                throw new NotFoundFavoritePostException("No favorite posts found");

            var result = _mapper.Map<List<PostDto>>(favoritePosts);
            return result;
        }

        public async Task<string> RemoveFromFavoriteAsync(AddToFavoriteDto favoritePostsDto)
        {
            var userId = GetUserId();

            var favorite = await _context.FavoritePosts
                       .FirstOrDefaultAsync(f => f.UserId == userId &&
                       f.PostId == favoritePostsDto.PostId);

            if (favorite == null)
                throw new NotFoundFavoritePostException("Favorite not found");

            _context.FavoritePosts.Remove(favorite);

            await _context.SaveChangesAsync();

            return "Post removed from favorites successfully";
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

