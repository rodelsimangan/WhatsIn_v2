using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public class ServiceReportsAppService : IServiceReportsAppService
    {
        IWhatsInDBContext _context;
        public ServiceReportsAppService(IWhatsInDBContext context)
        {
            _context = context;
        }

        public List<ReportModel> GetServiceReports(int ServiceId)
        {
            try
            {
                var query = from q in _context.Reports
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

        public ReportModel GetServiceReport(int ServiceReportId)
        {
            try
            {
                if (ServiceReportId == 0)
                    throw new NullReferenceException();

                var query = from q in _context.Reports
                            where q.Id == ServiceReportId
                            select q;

                return query.AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpsertServiceReport(ReportModel input)
        {
            try
            {
                if (input.Id == 0)
                    _context.Reports.Add(input);
                else
                    _context.Entry(input).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveServiceReport(int ServiceReportId)
        {
            try
            {
                if (ServiceReportId == 0)
                    throw new NullReferenceException();
                var Report = GetServiceReport(ServiceReportId);
                Report.IsDeleted = true;
                _context.Entry(Report).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}