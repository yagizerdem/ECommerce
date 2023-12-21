using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class UserProfile : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        
        public string? ProfileImgPath { get; set; }


    }
}
