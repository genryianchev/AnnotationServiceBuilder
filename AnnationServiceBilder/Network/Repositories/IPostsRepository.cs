using AnnationServiceBilder.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnnationServiceBilder.Network.Repositories
{
    public interface IPostsRepository
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
    }
}
