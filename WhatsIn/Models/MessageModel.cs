using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsIn.Models
{
    [Table("Messages")]
    public class MessageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Content { get; set; }
        public bool? IsDeleted { get; set; }
    }
}