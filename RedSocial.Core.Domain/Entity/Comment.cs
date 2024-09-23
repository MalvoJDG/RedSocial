namespace RedSocial.Core.Domain.Entity
{
    public class Comment
    {
        public int Id { get; set; } 
        public int PostID { get; set; } 
        public string UserID { get; set; } 
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }
        public int? ParentCommentID { get; set; }
        public Post Post { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment>? Replies { get; set; }
    }

}
