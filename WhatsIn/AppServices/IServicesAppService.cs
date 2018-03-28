using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public interface IServicesAppService
    {
        List<ServiceModel> GetServices(string UserId);
        List<ServiceModel> GetServices(string name, string location);
        List<ServiceModel> GetServices(string name, string location, int BlockNumber, int BlockSize);
        ServiceModel GetService(int ServiceId);
        int CountServices(string location, string type);
        void UpsertService(ServiceModel input);
        void RemoveService(int ServiceId);
    }
}
