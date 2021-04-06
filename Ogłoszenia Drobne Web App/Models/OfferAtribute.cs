using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Models
{
    public class OfferAtribute
    {
        public int OfferId { get; set; }

        public int AtributeId { get; set; }

        public string Value { get; set; }

        public virtual Offer Offer { get; set; }
        public virtual Atribute Atribute { get; set; }
    }
}