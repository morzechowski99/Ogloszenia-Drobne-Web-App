using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Models
{
    public class AppUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public AppUser()
        {
            Offers = new HashSet<Offer>();
        }

        [Required]
        [MaxLength(20, ErrorMessage = "Imię może mieć maksymalnie 20 znaków")]
        [Display(Name = "Imię")]
        [RegularExpression(@"^([A-Za-zzżźćńółęąśŻŹĆĄŚĘŁÓŃ]+)$", ErrorMessage = "Niepoprawne imię")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Nazwisko może mieć maksymalnie 50 znaków")]
        [Display(Name = "Nazwisko")]
        [RegularExpression(@"^([A-Za-zzżźćńółęąśŻŹĆĄŚĘŁÓŃ]+)$", ErrorMessage = "Niepoprawne nazwisko")]
        public string Surname { get; set; }

        [MaxLength(50, ErrorMessage = "Ulica może mieć maksymalnie 50 znaków")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Numer budynku")]
        public string BuldingNumber { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}