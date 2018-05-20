using Market.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Market.Web.Hangfire
{
    public class CheckOnlineUsersJob
    {
        private readonly IUserProfileService userProfileService;

        public CheckOnlineUsersJob(IUserProfileService userProfileService)
        {
            this.userProfileService = userProfileService;
        }

        public async Task Do(int orderId)
        {
           
        }
    }
}