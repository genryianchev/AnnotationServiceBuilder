using AnnotationServiceBuilder.Network.Services;
using System;
using System.Threading.Tasks;

namespace AnnotationServiceBuilder.Components
{
    public class MyComponent // Это может быть Blazor компонент или любой другой класс
    {
        private readonly IPostsService _postsService;

        public MyComponent(IPostsService postsService)
        {
            _postsService = postsService;
        }

        public async Task ShowPostsAsync()
        {
            var posts = await _postsService.GetPostsAsync();
            var post = await _postsService.GetPostByIdAsync(1);

            Console.WriteLine($"Total Posts: {posts.Count}");
            Console.WriteLine($"First Post Title: {post.Title}");
        }
    }
}
