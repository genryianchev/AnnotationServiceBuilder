using AnnotationServiceBuilder.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnnotationServiceBuilder.Network.Repositories
{
    public interface IPostsRepository
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
    }
}
