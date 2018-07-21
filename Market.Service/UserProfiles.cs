using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Market.Service
{
    public interface IUserProfileService
    {
        IEnumerable<UserProfile> GetUserProfiles();
        IQueryable<UserProfile> GetUserProfiles(Expression<Func<UserProfile, bool>> where, params Expression<Func<UserProfile, object>>[] includes);
        IQueryable<UserProfile> GetUserProfilesAsNoTracking(params Expression<Func<UserProfile, object>>[] includes);
        UserProfile GetUserProfile(Expression<Func<UserProfile, bool>> where, params Expression<Func<UserProfile, object>>[] includes);
        UserProfile GetUserProfileById(string id);
        UserProfile GetUserProfileByName(string name);
        void CreateUserProfile(UserProfile userProfile);
        void UpdateUserProfile(UserProfile userProfile);
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

        public IQueryable<UserProfile> GetUserProfiles(Expression<Func<UserProfile, bool>> where, params Expression<Func<UserProfile, object>>[] includes)
        {
            var userProfile = userProfilesRepository.GetManyAsNoTracking(where, includes);
            return userProfile;
        }

        public IQueryable<UserProfile> GetUserProfilesAsNoTracking(params Expression<Func<UserProfile, object>>[] includes)
        {
            var query = userProfilesRepository.GetAll(includes);
            return query;
        }


        public UserProfile GetUserProfileById(string id)
        {
            var userProfile = userProfilesRepository.GetUserById(id);
            return userProfile;
        }

        public UserProfile GetUserProfile(Expression<Func<UserProfile, bool>> where, params Expression<Func<UserProfile, object>>[] includes)
        {
            var userProfile = userProfilesRepository.Get(where, includes);
            return userProfile;
        }



        public void UpdateUserProfile(UserProfile userProfile)
        {
            userProfilesRepository.Update(userProfile);
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
