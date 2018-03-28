using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsIn.Models
{
    public class RatingViewModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string LoginName { get; set; }
        public string Comment { get; set; }
    }
}