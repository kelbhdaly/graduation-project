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

        public async Task<FavoritePostDto> AddToFavoriteAsync(AddToFavoriteDto addToFavoriteDto)
        {
            var userId = GetUserId();

            var exists = await _context.FavoritePosts
                .AnyAsync(f => f.UserId == userId && f.PostId == addToFavoriteDto.PostId);

            if (exists)
                throw new PostAlreadyInFavoritesException("Already added");

            var post = await _context.Posts
                .Where(p => p.Id == addToFavoriteDto.PostId)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    DoctorName = p.Doctor.ApplicationUser.UserName,
                    Images = p.PostImages.Select(i => i.ImageUrl).ToList()
                }).FirstOrDefaultAsync();

            if (post == null)
                throw new PostNotFoundException("Post not found");

            var favorite = new FavoritePost
            {
                UserId = userId,
                PostId = addToFavoriteDto.PostId
            };

            await _context.FavoritePosts.AddAsync(favorite);
            await _context.SaveChangesAsync();

            return new FavoritePostDto
            {
                Id = favorite.Id,
                PostId = post.Id,
                Title = post.Title,
                DoctorName = post.DoctorName!,
                Images = post.Images
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






        #region Private Method

        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new UnauthorizedException("User not authenticated");

            return userId;
        }

        #endregion


    }

}

