using RedSocial.Core.Application.ViewModels.Post;

namespace RedSocial.Core.Application.ViewModels.Friend
{
    public class HomeViewModel
    {
        public List<FriendViewModel> Friends { get; set; }
        public List<PostViewModel> Posts { get; set; }
    }
}
