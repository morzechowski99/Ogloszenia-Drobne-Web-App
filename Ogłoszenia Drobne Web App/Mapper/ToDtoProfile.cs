using AutoMapper;
using Ogłoszenia_Drobne_Web_App.Models;
using Ogłoszenia_Drobne_Web_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Mapper
{
    public class ToDtoProfile: Profile
    {
        public ToDtoProfile()
        {
            CreateMap<Offer, EditOfferViewModel>();
        }
    }
}
