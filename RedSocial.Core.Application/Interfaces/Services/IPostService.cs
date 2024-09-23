using RedSocial.Core.Application.ViewModels.Post;
using RedSocial.Core.Domain.Entity;

namespace RedSocial.Core.Application.Interfaces.Services
{
    public interface IPostService : IGenericService<SavePostViewModel, PostViewModel, Post>
    {
        Task<List<PostViewModel>> GetAllViewModelWithInclude();
        Task<List<PostViewModel>> GetAllFriendsPosts();

    }
}
