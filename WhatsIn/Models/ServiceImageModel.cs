using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsIn.Models
{
    [Table("ServiceImages")]
    public class ServiceImageModel
    {
        public int Id { get; set; }
        public string ServiceId { get; set; }
        public string ImagePath { get; set; }
        public bool? IsDeleted { get; set; }
    }
}