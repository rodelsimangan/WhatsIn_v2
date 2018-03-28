using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public interface IServiceImagesAppService
    {
        List<ServiceImageModel> GetServiceImages(int ServiceId);
        ServiceImageModel GetServiceImage(int ServiceImageId);
        void UpsertServiceImage(ServiceImageModel input);
        void RemoveServiceImage(int ServiceImageId);
    }
}
