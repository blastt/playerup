using Market.Service;
using Market.Web.SR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.SR.Mappings
{
    public class ConnectionMapping
    {
        private readonly List<SUser> _connections = new List<SUser>();
        private readonly IUserProfileService _userProfileService;

        public ConnectionMapping()
        {
            
        }

        public ConnectionMapping(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }
        public void Add(string key, string connectionId)
        {
            lock (_connections)
            {
                var sn = _connections.Where(x => x.Id == key).FirstOrDefault();
                if (sn != null) // there is a connection with this key
                {
                    _connections.Find(x => x.Id == key).ConnectionId.Add(connectionId);
                }
                else
                {
                    _connections.Add(new SUser { Id = key, ConnectionId = new List<string> { connectionId } });
                }
                var user = _userProfileService.GetUserProfileByName(key);
                if(user != null)
                {
                    user.IsOnline = true;
                    _userProfileService.SaveUserProfile();
                }
            }
        }
        //public List<SUser> GetConnections(string id)
        //{
        //    var con = _connections.Find(x => x.Id == id);
        //    return con != null ? con.ConnectionId : new List<SUser>();
        //}
        //public List<SUser> AllConnectionIds()
        //{
        //    List<SUser> results = new List<SUser>();
        //    var allItems = _connections.Where(x => x.ConnectionId.Count > 0).ToList();
        //    foreach (var item in allItems)
        //    {
        //        results.AddRange(item.ConnectionId);
        //    }
        //    return results;
        //}
        //public List<SUser> AllKeys()
        //{
        //    return _connections.Select(x => x.Id).ToList();
        //}
        public void Remove(string key, string connectionId)
        {
            lock (_connections)
            {
                var item = _connections.Find(x => x.Id == key);
                if (_connections.Find(x => x.Id == key) != null)
                {
                    _connections.Find(x => x.Id == key).ConnectionId.Remove(connectionId);
                    if (_connections.Find(x => x.Id == key).ConnectionId.Count == 0)
                    {
                        _connections.Remove(item);
                    }
                    var user = _userProfileService.GetUserProfileByName(key);
                    if (user != null)
                    {
                        user.IsOnline = false;
                        _userProfileService.SaveUserProfile();
                    }
                }
            }
        }
    }
}