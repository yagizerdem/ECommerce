using Entity.EntityClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class UserProfileModel
    {
        public UserProfile userProfile {  get; set; }
        public List<Comment> comments { get; set; }
    }
}
