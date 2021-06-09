using Ogłoszenia_Drobne_Web_App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.ViewModels
{
    public class EditOfferViewModel
    {
        public int Id { get; set; }

        [MaxLength(40, ErrorMessage = "Tytuł może mieć maksymalnie 40 znaków")]
        [Required(ErrorMessage = "Tytuł jest obowiązkowy")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "Opis może mieć maksymalnie 1500 znaków")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Stawka")]
        [DataType(DataType.Currency)]
        public decimal? Wage { get; set; }
        public virtual ICollection<OfferAtribute> OfferAtributes { get; set; }
        public virtual Category Category { get; set; }
    }
}
