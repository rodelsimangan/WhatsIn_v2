using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public class ServiceImagesAppService : IServiceImagesAppService
    {
        IWhatsInDBContext _context;
        public ServiceImagesAppService(IWhatsInDBContext context)
        {
            _context = context;
        }
        public List<ServiceImageModel> GetServiceImages(int ServiceId)
        {
            try
            {
                var query = from q in _context.ServiceImages
                            where q.IsDeleted == false || q.IsDeleted == null
                            && q.ServiceId == ServiceId.ToString()
                            select q;

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ServiceImageModel GetServiceImage(int ServiceImageId)
        {
            try
            {
                if (ServiceImageId == 0)
                    throw new NullReferenceException();

                var query = from q in _context.ServiceImages
                            where q.Id == ServiceImageId
                            select q;

                return query.AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpsertServiceImage(ServiceImageModel input)
        {
            try
            {
                if (input.Id == 0)
                    _context.ServiceImages.Add(input);
                else
                    _context.Entry(input).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveServiceImage(int ServiceImageId)
        {
            try
            {
                if (ServiceImageId == 0)
                    throw new NullReferenceException();
                var service = GetServiceImage(ServiceImageId);
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