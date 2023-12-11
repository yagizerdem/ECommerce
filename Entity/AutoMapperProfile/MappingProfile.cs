using AutoMapper;
using Entity.EntityClass;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.AutoMapperProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<SingInViewModel, AppUser>();
            CreateMap<BookModel , Book>();
        }
    }
}
