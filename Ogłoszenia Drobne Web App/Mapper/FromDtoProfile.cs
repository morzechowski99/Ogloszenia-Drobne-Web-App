using AutoMapper;
using Ogłoszenia_Drobne_Web_App.Models;
using Ogłoszenia_Drobne_Web_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Mapper
{
    public class FromDtoProfile : Profile
    {
        public FromDtoProfile()
        {
            CreateMap<CreateCategoryViewModel, Category>()
                .ForMember(c => c.CategoryName,map => map.MapFrom(v => v.CategoryName))
                .ForMember(c => c.ParentCategoryId,map => map.MapFrom(v => v.ParentCategoryId))
                .ForAllOtherMembers(map => map.Ignore());
        }
    }
}
