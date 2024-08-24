using AnnationServiceBilder.Annotations.Refit;
using AnnationServiceBilder.Data.Models;
using Refit;

namespace AnnationServiceBilder.Network.Repositories
{
    [RefitClient]
    public interface IPostsApi
    {
        [Get("/posts")]
        Task<List<Post>> GetPostsAsync();

        [Get("/posts/{id}")]
        Task<Post> GetPostByIdAsync(int id);
    }
}
