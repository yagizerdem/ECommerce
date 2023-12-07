using Entity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.EntityClass
{
    public class BaseEntity : IEntity
    {
        public int Id { get ; set ; }
        public int CreatedIp { get ; set ; }
        public int UpdatedIp { get ; set ; }
    }
}
