using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RedSocial.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        public int Id { get; set; }
        public string? User_Id { get; set; }

        [Required(ErrorMessage = "You must to choose an option")]
        [DataType(DataType.Text)]
        public string PublicationType { get; set; }
        public DateTime PostDate { get; set; }

        [Required(ErrorMessage = "A Body es necesary")]
        [DataType(DataType.Text)]
        public string? Body { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }
        public string? Archive { get; set; }
    }
}
