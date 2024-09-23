using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSocial.Core.Application.ViewModels.Comments
{
    public class CommentsViewModel
    {
        public int Id { get; set; }
        public int PostID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; } 
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }
        public int? ParentCommentID { get; set; }
        public List<CommentsViewModel> Replies { get; set; } 
    }
}
