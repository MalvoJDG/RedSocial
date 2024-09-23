namespace RedSocial.Core.Application.ViewModels.Comments
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; } 
        public string UserID { get; set; } 
        public string Content { get; set; }
        public int? ParentCommentID { get; set; } 
    }
}
