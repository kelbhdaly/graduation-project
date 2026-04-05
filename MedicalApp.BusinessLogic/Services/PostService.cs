namespace MedicalApp.BusinessLogic.Services
{
    public class PostService : IPostService
    {
        private readonly IImageService _imageService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostService(IImageService imageService
           , ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _imageService = imageService;
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<PostDto>> GetAllPostsAsync()
        {
            var posts = await _context.Posts.Include(p => p.PostImages)
                             .Include(p => p.Doctor)
                            .ThenInclude(d => d.ApplicationUser).ToListAsync();

            return _mapper.Map<List<PostDto>>(posts);
        }

        public async Task<PostDto> CreatePostAsync(CreatePostDto createPostDto)
        {
            var userId = GetUserId();

            var doctor = await _context.Doctors
                                .Include(d => d.ApplicationUser)
                                 .FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null)
                throw new UnauthorizedException("Only doctors can create posts");

            if (createPostDto.Images != null && createPostDto.Images.Count > 3)
            {
                throw new InvalidFormatException("Maximum allowed images is 3");
            }

            var imageUrls = new List<string>();

            if (createPostDto.Images != null && createPostDto.Images.Count > 0)
            {
                imageUrls = await _imageService.UploadImagesAsync(createPostDto.Images, "posts");
            }

            var post = new Post
            {
                Title = createPostDto.Title,
                Description = createPostDto.Description,
                PostImages = imageUrls.Select(x => new PostImage
                {
                    ImageUrl = x
                }).ToList(),
                DoctorId = doctor.Id

            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return new PostDto
            {
                Description = post.Description,
                Title = post.Title,
                Id = post.Id,
                DoctorName = doctor.ApplicationUser.UserName!,
                ImageUrls = post.PostImages.Select(x => new PostImageDto
                {
                    ImageUrl = x.ImageUrl
                }).ToList()
            };
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var userId = GetUserId();
            var isAdmin = _httpContextAccessor.HttpContext.User.IsInRole("ADMIN");
            var post = await _context.Posts.Include(p => p.PostImages)
                .Include(p => p.Doctor).FirstOrDefaultAsync(p => p.Id == postId);

            if (!isAdmin && post!.Doctor.UserId != userId)
                throw new UnauthorizedException("You can't delete this post");


            if (post == null)
                throw new PostNotFoundException("Post not found");

            foreach (var image in post.PostImages)
            {
                await _imageService.DeleteImageAsync(image.ImageUrl);
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<int> UpdatePostAsync(int postId, UpdatePostDto updatePostDto)
        {
            var userId = GetUserId();

            var post = await _context.Posts.Include(p => p.PostImages)
                                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post?.Doctor.UserId != userId)
                throw new UnauthorizedException("You can't delete this post");

            if (post == null)
                throw new PostNotFoundException("Post not found");

            post.Title = updatePostDto.Title;
            post.Description = updatePostDto.Description;

            if (updatePostDto.Images != null && updatePostDto.Images.Count > 3)
                throw new InvalidFormatException("Maximum allowed images is 3");

            if (updatePostDto.Images != null && updatePostDto.Images.Count > 0)
            {
                foreach (var image in post.PostImages)
                {
                    await _imageService.DeleteImageAsync(image.ImageUrl);
                }

                _context.PostImages.RemoveRange(post.PostImages);

                var imageUrls = await _imageService.UploadImagesAsync(updatePostDto.Images, "posts");

                post.PostImages = imageUrls.Select(x => new PostImage
                {
                    ImageUrl = x
                }).ToList();
            }

            await _context.SaveChangesAsync();

            return post.Id;
        }



        #region Private Method

        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User
                       .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new UnauthorizedException("User not authenticated");

            return userId;
        }
        #endregion
    }
}
