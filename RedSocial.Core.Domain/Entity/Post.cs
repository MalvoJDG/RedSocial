namespace RedSocial.Core.Domain.Entity
{
    public class Post
    {
        public int Id { get; set; }
        public string User_Id {  get; set; }
        public string PublicationType { get; set; }
        public DateTime PostDate { get; set; }
        public string? Body { get; set; }
        public string? Archive { get; set; }

        public ICollection<Comment> Comments { get; set; }

    }
}
