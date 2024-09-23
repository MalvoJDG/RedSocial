using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSocial.Core.Application.ViewModels.Friend
{
    public class SaveFriendViewModel
    {
        public int Id { get; set; }
        public string? User_Id1 { get; set; }
        public string User_Id2 { get; set; }

        [Required(ErrorMessage = "You must to choose an option")]
        [DataType(DataType.Text)]
        public string FriendUser { get; set; }
    }
}
