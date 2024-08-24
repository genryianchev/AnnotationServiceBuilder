using AnnationServiceBilder.Annotations.Scoped;
using AnnationServiceBilder.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnnationServiceBilder.Network.Repositories.Impl
{
    [ScopedService(typeof(IPostsRepository))]
    public class PostsRepository : IPostsRepository
    {
        private readonly IPostsApi _postsApi;

        public PostsRepository(IPostsApi postsApi)
        {
            _postsApi = postsApi;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _postsApi.GetPostsAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _postsApi.GetPostByIdAsync(id);
        }
    }
}
