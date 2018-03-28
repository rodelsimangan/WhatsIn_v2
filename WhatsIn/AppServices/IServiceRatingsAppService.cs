using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public interface IServiceRatingsAppService
    {
        List<RatingModel> GetServiceRatings(int ServiceId);
        RatingModel GetServiceRating(int RatingId);
        void UpsertServiceRating(RatingModel input);
        void RemoveServiceRating(int RatingId);
    }
}
