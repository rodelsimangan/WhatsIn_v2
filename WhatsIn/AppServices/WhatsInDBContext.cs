using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public class WhatsInDBContext : DbContext, IWhatsInDBContext
    {
        public WhatsInDBContext() :
            base("DefaultConnection")
        {
        }

        public DbSet<ServiceModel> Services { get; set; }
        public DbSet<ServiceImageModel> ServiceImages { get; set; }
        public DbSet<RatingModel> Ratings { get; set; }
        public DbSet<WatchlistViewModel> Watchlists { get; set; }
        public DbSet<ReportModel> Reports { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<ItineraryViewModel> Itineraries { get; set; }
    }
}