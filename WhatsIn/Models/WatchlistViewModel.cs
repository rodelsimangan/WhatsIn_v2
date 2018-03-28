using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsIn.Models
{
    [Table("Watchlists")]
    public class WatchlistViewModel
    {
        public int Id { get; set; }
        public string ServiceId { get; set; }
        public string LoginId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}