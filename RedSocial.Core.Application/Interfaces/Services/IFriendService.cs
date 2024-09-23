using RedSocial.Core.Application.ViewModels.Friend;
using RedSocial.Core.Application.ViewModels.Post;
using RedSocial.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSocial.Core.Application.Interfaces.Services
{
    public interface IFriendService : IGenericService<SaveFriendViewModel, FriendViewModel, Friend>
    {
        Task<List<FriendViewModel>> GetAllViewModelWithInclude();

    }
}
