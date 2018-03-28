using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsIn.Models
{
    [Table("Ratings")]
    public class RatingModel
    {
        public int Id { get; set; }
        public string LoginId { get; set; }
        public int ServiceId { get; set; }
        public bool Rating { get; set; }
        public string Comment { get; set; }
        public bool? IsDeleted { get; set; }
    }
}