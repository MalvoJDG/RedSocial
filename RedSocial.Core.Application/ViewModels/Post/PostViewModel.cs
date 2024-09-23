using RedSocial.Core.Application.ViewModels.Comments;

namespace RedSocial.Core.Application.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string User_Id { get; set; }
        public string UserName { get; set; }
        public string? UserPhoto { get; set; }
        public string PublicationType { get; set; }
        public DateTime PostDate { get; set; }
        public string? Body { get; set; }
        public string? Archive { get; set; }

        public ICollection<CommentsViewModel>? Comments { get; set; }

    }
}
