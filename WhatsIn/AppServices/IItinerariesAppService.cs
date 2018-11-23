using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public interface IItinerariesAppService
    {
        List<ItineraryViewModel> GetItineraries(int UserId);
        ItineraryViewModel GetItinerary(int ItineraryId);
        void UpsertItinerary(ItineraryViewModel input);
        void RemoveItinerary(int ItineraryId);
    }
}
