using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Market.Service;
using Market.Web.SR.Models;
using Microsoft.AspNet.SignalR;

namespace Market.Web.SignalR.MyHubs
{
    public class StateHub : Hub
    {
        private readonly IUserProfileService _userProfileService;
        
        //static List<SUser> Users = new List<SUser>();

        public void SetOnline()
        {
            //string name = Context.User.Identity.Name;
            //var user = _userProfileService.GetUserProfileByName(Context.User.Identity.Name);
            //if (user != null)
            //{
            //    user.IsOnline = true;
            //    _userProfileService.SaveUserProfile();
            //}
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            var user = _userProfileService.GetUserProfileByName(Context.User.Identity.Name);
            if (user != null)
            {
                user.IsOnline = true;
                _userProfileService.SaveUserProfile();
            }

            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            var user = _userProfileService.GetUserProfileByName(Context.User.Identity.Name);
            if (user != null)
            {
                user.IsOnline = false;
                _userProfileService.SaveUserProfile();
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}