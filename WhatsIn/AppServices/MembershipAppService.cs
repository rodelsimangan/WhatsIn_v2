using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatsIn.Models;

namespace WhatsIn.AppServices
{
    public class MembershipAppService : IMembershipAppService
    {
        ApplicationDbContext _userContext;

        public MembershipAppService()
        {
            _userContext = new ApplicationDbContext();
        }
        public string GetUserId(string UserName)
        {
            try
            {
                ApplicationUser user = new ApplicationUser();
                
                var query = from u in _userContext.Users
                           where u.Email == UserName
                           select u;
                user = query.FirstOrDefault();
                return user.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApplicationUser GetUser(string UserId)
        {
            try
            {
                var query = from u in _userContext.Users
                            where u.Id == UserId
                            select u;
                var user = query.FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}