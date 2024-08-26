namespace AnnotationServiceBuilder.Network.Services.Impl
{
    using AnnotationServiceBuilder.Annotations.Scoped;
    using AnnotationServiceBuilder.Data.Models;
    using AnnotationServiceBuilder.Network.Repositories;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ScopedService(typeof(IPostsService))]
    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _postsRepository;

        public PostsService(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _postsRepository.GetPostsAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _postsRepository.GetPostByIdAsync(id);
        }
    }

}
