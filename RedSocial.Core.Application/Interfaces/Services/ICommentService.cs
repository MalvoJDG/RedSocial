using RedSocial.Core.Application.ViewModels.Comments;
using RedSocial.Core.Application.ViewModels.Post;
using RedSocial.Core.Domain.Entity;

namespace RedSocial.Core.Application.Interfaces.Services
{
    public interface ICommentService : IGenericService<SaveCommentViewModel, CommentsViewModel, Comment>
    {

    }
}
