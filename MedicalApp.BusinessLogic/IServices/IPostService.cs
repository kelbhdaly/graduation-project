namespace MedicalApp.BusinessLogic.IServices
{
    public interface IPostService
    {
        Task<PostDto> CreatePostAsync(CreatePostDto createPostDto);
        Task<List<PostDto>> GetAllPostsAsync();
        Task<bool> DeletePostAsync(int postId);
        Task<int> UpdatePostAsync(int postId, UpdatePostDto updatePostDto);
    }
}
