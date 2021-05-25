using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ogłoszenia_Drobne_Web_App.Models;

namespace Ogłoszenia_Drobne_Web_App.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Display(Name = "Nazwa")]
        [MaxLength(20, ErrorMessage = "Nazwa kategorii nie może być dłuższa niż 20 znaków")]
        [Required(ErrorMessage ="Nazwa jest wymagana")]
        public string CategoryName { get; set; }
        [Display(Name = "Kategoria nadrzędna")]
        public int? ParentCategoryId { get; set; }
        public List<CategoryAtributeViewModel> Attributes { get; set; }
    }
}
