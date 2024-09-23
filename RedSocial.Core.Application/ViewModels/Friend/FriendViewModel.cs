using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSocial.Core.Application.ViewModels.Friend
{
    public class FriendViewModel
    {
        public int Id { get; set; }
        public string User_Id1 { get; set; }
        public string User_Id2 { get; set; }
        public string FirstName { get; set; }  
        public string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string UserName { get; set; }  
    }
}
