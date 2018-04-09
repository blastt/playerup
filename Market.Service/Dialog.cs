﻿using Market.Data.Infrastructure;
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
        int UnreadDialogsForUserCount(string userId);
        int UnreadMessagesInDialogCount(Dialog dialog);
    }

    public class DialogService : IDialogService
    {
        private readonly IDialogRepository dialogsRepository;
        private readonly IUnitOfWork unitOfWork;

        public DialogService(IDialogRepository dialogsRepository, IUnitOfWork unitOfWork)
        {
            this.dialogsRepository = dialogsRepository;
            this.unitOfWork = unitOfWork;
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

        public int UnreadDialogsForUserCount(string userId)
        {
            int unreadDialogsCount = 0;
            foreach (var d in GetDialogs().Where(d => d.Users.Any(u => u.Id == userId)))
            {
                if (d.Messages != null)
                {

                    if (d.Messages.Any(m => !m.ToViewed && m.ReceiverId == userId))
                    {
                        unreadDialogsCount++;
                    }

                }

            }
            return unreadDialogsCount;
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


