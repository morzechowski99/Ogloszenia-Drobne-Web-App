using Ogłoszenia_Drobne_Web_App.Models.Enums;
using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        public AtributeType Type { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<OfferAtribute> OfferAtributes { get; set; }
    }
}