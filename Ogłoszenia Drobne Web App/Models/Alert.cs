using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public String Content { get; set; }
        public DateTime CreateDate { get; set; }
    }
}