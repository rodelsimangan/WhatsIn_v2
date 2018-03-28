using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public class ServiceRatingsAppService : IServiceRatingsAppService
    {
        IWhatsInDBContext _context;
        public ServiceRatingsAppService(IWhatsInDBContext context)
        {
            _context = context;
        }

        public List<RatingModel> GetServiceRatings(int ServiceId)
        {
            try
            {
                var query = from q in _context.Ratings
                            where q.IsDeleted == false || q.IsDeleted == null
                            && q.ServiceId == ServiceId
                            select q;

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RatingModel GetServiceRating(int ServiceRatingId)
        {
            try
            {
                if (ServiceRatingId == 0)
                    throw new NullReferenceException();

                var query = from q in _context.Ratings
                            where q.Id == ServiceRatingId
                            select q;

                return query.AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpsertServiceRating(RatingModel input)
        {
            try
            {
                if (input.Id == 0)
                    _context.Ratings.Add(input);
                else
                    _context.Entry(input).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveServiceRating(int ServiceRatingId)
        {
            try
            {
                if (ServiceRatingId == 0)
                    throw new NullReferenceException();
                var rating = GetServiceRating(ServiceRatingId);
                rating.IsDeleted = true;
                _context.Entry(rating).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}