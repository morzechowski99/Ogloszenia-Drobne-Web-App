using Ogłoszenia_Drobne_Web_App.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Models
{
    public class Atribute
    {
        public Atribute()
        {
            OfferAtributes = new HashSet<OfferAtribute>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Display(Name ="Nazwa")]
        public string Name { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<OfferAtribute> OfferAtributes { get; set; }
    }
}