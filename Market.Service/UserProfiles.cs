using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Service
{
    public interface IUserProfileService
    {
        IEnumerable<UserProfile> GetUserProfiles();
        UserProfile GetUserProfileById(string id);
        UserProfile GetUserProfileByName(string name);
        void CreateUserProfile(UserProfile userProfile);
        void SaveUserProfile();
    }

    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository userProfilesRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserProfileService(IUserProfileRepository userProfilesRepository, IUnitOfWork unitOfWork)
        {
            this.userProfilesRepository = userProfilesRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IUserProfileService Members

        public IEnumerable<UserProfile> GetUserProfiles()
        {
            var userProfile = userProfilesRepository.GetAll();
            return userProfile;
        }


        public UserProfile GetUserProfileById(string id)
        {
            var userProfile = userProfilesRepository.GetUserById(id);
            return userProfile;
        }

        public UserProfile GetUserProfileByName(string name)
        {
            var userProfile = userProfilesRepository.GetUserByName(name);
            return userProfile;
        }

        public void CreateUserProfile(UserProfile userProfile)
        {
            userProfilesRepository.Add(userProfile);
        }

        public void SaveUserProfile()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}
