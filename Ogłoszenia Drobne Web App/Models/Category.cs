using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ogłoszenia_Drobne_Web_App.Models
{
    public class Category
    {
        public Category()
        {
            Offer = new HashSet<Offer>();
        }

        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [MaxLength(20, ErrorMessage = "Nazwa kategorii nie może być dłuższa niż 20 znaków")]
        public string CategoryName { get; set; }

        public int ParentCategoryId { get; set; }
        public virtual ICollection<Offer> Offer { get; set; }
        public virtual Category ParentCategory { get; set; }
    }
}