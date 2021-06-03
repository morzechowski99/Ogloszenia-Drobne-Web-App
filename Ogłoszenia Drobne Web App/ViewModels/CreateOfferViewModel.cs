using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.ViewModels
{
    public class CreateOfferViewModel
    {

        [Display(Name = "Kategoria")]
        [Min(0, ErrorMessage = "Kategoria jest wymagana")]
        public int CategoryId { get; set; }

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

        public List<CreateOfferAttribute> Attributes { get; set; }
    }
}
