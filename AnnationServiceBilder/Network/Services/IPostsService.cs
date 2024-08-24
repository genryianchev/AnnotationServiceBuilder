using AnnationServiceBilder.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnnationServiceBilder.Network.Services
{

    public interface IPostsService
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
    }
}
