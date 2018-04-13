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

    public interface IDialogService
    {
        IEnumerable<Dialog> GetDialogs();
        Dialog GetDialog(int id);
        void CreateDialog(Dialog message);
        void SaveDialog();
        Dialog GetPrivateDialog(UserProfile user1, UserProfile user2);
        string GetOtherUserInDialog(int dialogId, string userId);
        IEnumerable<Dialog> GetUserDialogs(string userId);
        int UnreadDialogsForUserCount(string userId);
        int UnreadMessagesInDialogCount(Dialog dialog);
    }

    public class DialogService : IDialogService
    {
        private readonly IDialogRepository dialogsRepository;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IUnitOfWork unitOfWork;

        public DialogService(IDialogRepository dialogsRepository, IUnitOfWork unitOfWork, IUserProfileRepository userProfileRepository)
        {
            this.dialogsRepository = dialogsRepository;
            this.unitOfWork = unitOfWork;
            this.userProfileRepository = userProfileRepository;
        }

        #region IDialogService Members

        public IEnumerable<Dialog> GetDialogs()
        {
            var dialogs = dialogsRepository.GetAll();
            return dialogs;
        }


        public Dialog GetDialog(int id)
        {
            var dialog = dialogsRepository.GetById(id);
            return dialog;
        }
        public void CreateDialog(Dialog dialog)
        {
            dialogsRepository.Add(dialog);
        }

        public Dialog GetPrivateDialog(UserProfile user1, UserProfile user2)
        {
            var user1Dialogs = user1.DialogsAsCreator.Concat(user1.DialogsAsСompanion);
             
            var user2Dialogs = user2.DialogsAsCreator.Concat(user2.DialogsAsСompanion);

            foreach (var d in user1Dialogs)
            {
                if (user2Dialogs.Contains(d))
                {
                    return d;
                }
            }
            return null;
        }

        public string GetOtherUserInDialog(int dialogId, string userId)
        {
            string otherUserId = null;
            var dialog = dialogsRepository.GetById(dialogId);
            if (dialog != null)
            {
                if (dialog.CompanionId == userId)
                {
                    otherUserId = dialog.CreatorId;
                }
                else if (dialog.CreatorId == userId)
                {
                    otherUserId = dialog.CompanionId;
                }
            }
            
            return otherUserId;
        }

        public int UnreadDialogsForUserCount(string userId)
        {
            int unreadDialogsCount = 0;
            var user = userProfileRepository.GetUserById(userId);
            if (user != null)
            {
                var creatorDialogs = user.DialogsAsCreator;
                foreach (var dialog in creatorDialogs)
                {
                    if (dialog.Messages.Any(m => !m.ToViewed && m.ReceiverId == userId))
                    {
                        unreadDialogsCount++;
                    }
                }

                var companionDialogs = user.DialogsAsСompanion;
                foreach (var dialog in companionDialogs)
                {
                    if (dialog.Messages.Any(m => !m.ToViewed && m.ReceiverId == userId))
                    {
                        unreadDialogsCount++;
                    }
                }
            }
            return unreadDialogsCount;
        }

        public IEnumerable<Dialog> GetUserDialogs(string userId)
        {
            var dialogs = dialogsRepository.GetMany(d => d.CompanionId == userId || d.CreatorId == userId);
            return dialogs;
        }

        public int UnreadMessagesInDialogCount(Dialog dialog)
        {
            int messagesInDialogCount = 0;
            foreach (var m in dialog.Messages)
            {
                if (!m.ToViewed)
                {
                    messagesInDialogCount++;
                }
            }
            return messagesInDialogCount;
        }

        public void SaveDialog()
        {
            unitOfWork.Commit();
        }

        #endregion

    }

}


