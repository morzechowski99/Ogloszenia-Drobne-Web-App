using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Models
{
    public class Offer
    {
        public Offer()
        {
            OfferAtributes = new HashSet<OfferAtribute>();
            Files = new HashSet<File>();
        }

        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Display(Name = "Kategoria")]
        [Min(0,ErrorMessage ="Kategoria jest wymagana")]
        public int CategoryId { get; set; }

        [MaxLength(40, ErrorMessage = "Tytuł może mieć maksymalnie 40 znaków")]
        [Required(ErrorMessage = "Tytuł jest obowiązkowy")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "Opis może mieć maksymalnie 1500 znaków")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Data utworzenia")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Data ostatniej zmiany")]
        public DateTime? LastModificationDate { get; set; }

        [Display(Name = "Data wygaśnięcia")]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = "Ilość wyświetleń")]
        public int ViewCounter { get; set; }

        [Display(Name = "Stawka")]
        [DataType(DataType.Currency)]
        public decimal? Wage { get; set; }

        public bool reported { get; set; }

        //[NotMapped]
        //[RegularExpression(@"^([0-9]*)([.,]*)([0-9]\d{0,1})$", ErrorMessage = "Zły format")]
        //[Display(Name = "Stawka")]
        //public string WageValue { get; set; }
        public virtual Category Category { get; set; }

        public virtual AppUser User { get; set; }

        //public virtual ICollection<OfferReport> OfferReports { get; set; }
        public virtual ICollection<OfferAtribute> OfferAtributes { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}