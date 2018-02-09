using Market.Data.Infrastructure;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Repositories
{
    public class UserProfileRepository : RepositoryBase<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(IDbFactory dbFactory)
            : base(dbFactory) { }



        public UserProfile GetUserById(string userId)
        {
            return DbContext.UserProfiles.Where(u => u.Id == userId).FirstOrDefault();
        }

        public UserProfile GetUserByName(string userName)
        {
            return DbContext.UserProfiles.Where(u => u.Name == userName).FirstOrDefault();
        }

    }

    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        UserProfile GetUserByName(string userName);
        UserProfile GetUserById(string userId);

    }
}
