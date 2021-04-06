using Ogłoszenia_Drobne_Web_App.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Models
{
    public class OfferReport
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public int ReportingUserId { get; set; }
        public DateTime ReportDate { get; set; }
        public ReportReason Reason { get; set; }
        public bool IsActive { get; set; }
        public virtual AppUser ReportingUser { get; set; }
        public virtual Offer Offer { get; set; }
    }
}