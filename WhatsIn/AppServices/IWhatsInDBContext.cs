using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public interface IWhatsInDBContext
    {
        DbSet<ServiceModel> Services { get; set; }
        DbSet<ServiceImageModel> ServiceImages { get; set; }
        DbSet<RatingModel> Ratings { get; set; }
        DbSet<ReportModel> Reports { get; set; }
        DbSet<WatchlistViewModel> Watchlists { get; set; }
        DbSet<MessageModel> Messages { get; set; }
        DbSet<ItineraryViewModel> Itineraries { get; set; }

        int SaveChanges();
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbEntityEntry Entry(object entity);
        Database Database { get; }

    }
}
