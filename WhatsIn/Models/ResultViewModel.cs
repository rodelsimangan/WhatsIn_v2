using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WhatsIn.Models
{
    public class ResultViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceProvides { get; set; }
        public string ServiceLocation { get; set; }
        public string ServicePhoneNumber { get; set; }
        public string ServiceSchedule { get; set; }
        public string ServiceFacebookPage { get; set; }
        public string ServiceTwitterPage { get; set; }
        public string ServiceInstagramPage { get; set; }
        public string ServiceWebsiteAddress { get; set; }
        public string[] ServiceImagesPath { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserNameIdentifier { get; set; }
        public string UserAddress { get; set; }
        public string UserLocation { get; set; }
        public string UserContactNumber { get; set; }

        public int PositiveRatings { get; set; }
        public int NegativeRatings { get; set; }

        public bool RatingGiven { get; set; }
        public string LoginId { get; set; }
    }

    public class JsonModel
    {
        public string HTMLString { get; set; }
        public bool NoMoreData { get; set; }
    }
}