using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.ViewModels
{
    public class CategoryWithParentCategory
    {
        public string Name { get; set; }
        public CategoryWithParentCategory Parent { get; set; }
    }
}
