namespace MedicalApp.BusinessLogic.ValueResolver
{
    public class PostImageUrlResolver : IValueResolver<PostImage, PostImageDto, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostImageUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string Resolve(PostImage source, PostImageDto destination, string destMember, ResolutionContext context)
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            if (request == null)
                return source.ImageUrl;

            var baseUrl = $"{request.Scheme}://{request.Host}";

            return $"{baseUrl}{source.ImageUrl}";
        }
    }
}
