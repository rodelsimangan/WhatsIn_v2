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
        string GetUserId(string UserName);
        ApplicationUser GetUser(string UserId);
    }
}
