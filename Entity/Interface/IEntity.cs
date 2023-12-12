using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Interface
{
    public interface IEntity
    {
        public int Id { get; set; } 
        
        public int CreatedIp { get; set; }

        public int UpdatedIp { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
