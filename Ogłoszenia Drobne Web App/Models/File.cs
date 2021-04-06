using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Models
{
    public class File
    {
        public int Id { get; set; }
        public int OfferId { get; set; }

        public byte[] Content { get; set; }
        public string Extension { get; set; }
        public virtual Offer Offer { get; set; }
    }
}