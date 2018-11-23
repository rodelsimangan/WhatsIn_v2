using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsIn.Models
{
    [Table("Itineraries")]
    public class ItineraryViewModel
    {
        public int Id { get; set; }
        public string ServiceId { get; set; }
        public string LoginId { get; set; }
        public int Sequence { get; set; }
        public DateTime? ITDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsCompleted { get; set; }
    }
}