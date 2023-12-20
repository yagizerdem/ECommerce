using Entity.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Entity.EntityClass
{
    public class BaseEntity : IEntity
    {
        public BaseEntity()
        {
            CreatedIp = IpFinder.GetIp();
            CreateDate = DateTime.Now;
        }
        public int Id { get ; set ; }
        public string CreatedIp { get ; set ; }
        public string? UpdatedIp { get ; set ; }

        public DateTime CreateDate { get ; set ; }
    }
}
