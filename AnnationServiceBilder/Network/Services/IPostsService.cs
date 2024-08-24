using AnnationServiceBilder.Data.Models;

namespace AnnationServiceBilder.Network.Services
{

    public interface IPostsService
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
    }
}
