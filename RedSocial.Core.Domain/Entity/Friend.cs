using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSocial.Core.Domain.Entity
{
    public class Friend
    {
        public int Id { get; set; }
        public string User_Id1 { get; set; }
        public string User_Id2{ get; set; }
    }
}
