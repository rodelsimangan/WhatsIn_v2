using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsIn.Models
{
    [Table("Services")]
    public class ServiceModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ServiceProvides { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Schedule { get; set; }
        public string FacebookPage { get; set; }
        public string TwitterPage { get; set; }
        public string InstagramPage { get; set; }
        public string WebsiteAddress { get; set; }
        public bool? IsDeleted { get; set; }
    }
}