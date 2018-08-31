using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public interface IMembershipAppService
    {
        ApplicationUser GetUser(string UserId);
        void UpdateUser(ApplicationUser input);
    }
}
