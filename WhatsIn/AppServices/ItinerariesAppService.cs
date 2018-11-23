using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public class ItinerariesAppService : IItinerariesAppService
    {
        IWhatsInDBContext _context;
        public ItinerariesAppService(IWhatsInDBContext context)
        {
            _context = context;
        }
        public List<ItineraryViewModel> GetItineraries(int UserId)
        {
            try
            {
                var query = from q in _context.Itineraries
                            where q.IsDeleted == false || q.IsDeleted == null
                            && q.LoginId == UserId.ToString()
                            select q;

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ItineraryViewModel GetItinerary(int ItineraryId)
        {
            try
            {
                if (ItineraryId == 0)
                    throw new NullReferenceException();

                var query = from q in _context.Itineraries
                            where q.Id == ItineraryId
                            select q;

                return query.AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpsertItinerary(ItineraryViewModel input)
        {
            try
            {
                if (input.Id == 0)
                    _context.Itineraries.Add(input);
                else
                    _context.Entry(input).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveItinerary(int ItineraryId)
        {
            try
            {
                if (ItineraryId == 0)
                    throw new NullReferenceException();
                var service = GetItinerary(ItineraryId);
                service.IsDeleted = true;
                _context.Entry(service).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}