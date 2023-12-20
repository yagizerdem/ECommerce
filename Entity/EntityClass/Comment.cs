using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class Comment : BaseEntity
    {
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        
        public string Message { get; set; }
    }
}
