using AnnationServiceBilder.Data.Models;

namespace AnnationServiceBilder.Network.Repositories
{
    public interface IPostsRepository
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
    }
}
