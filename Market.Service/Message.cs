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
    public interface IMessageService
    {
        IEnumerable<Message> GetMessages();
        Message GetMessage(int id);
        void SetMessageViewed(int id);
        void CreateMessage(Message message);
        void DeleteMessage(int id);
        void SaveMessage();
    }

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messagesRepository;
        private readonly IUnitOfWork unitOfWork;

        public MessageService(IMessageRepository messagesRepository, IUnitOfWork unitOfWork)
        {
            this.messagesRepository = messagesRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IUserProfileService Members

        public IEnumerable<Message> GetMessages()
        {
            var message = messagesRepository.GetAll();
            return message;
        }


        public Message GetMessage(int id)
        {
            var message = messagesRepository.GetById(id);
            return message;
        }


        public void CreateMessage(Message message)
        {
            var prevMessages = messagesRepository.GetMany(p => (p.ReceiverId == message.ReceiverId && p.SenderId == message.SenderId) ||
            (p.ReceiverId == message.SenderId && p.SenderId == message.ReceiverId));

            // if true
            if (prevMessages.Count() != 0)
            {
                message.ParentMessageId = prevMessages.LastOrDefault().Id;
            }
            
            messagesRepository.Add(message);
            
            
        }

        public void SaveMessage()
        {
            unitOfWork.Commit();
        }

        public void SetMessageViewed(int id)
        {
            var message = messagesRepository.GetById(id);
            if (message != null)
            {
                message.IsViewed = true;
            }
        }

        public void DeleteMessage(int id)
        {
            var message = messagesRepository.GetById(id);
            if (message != null)
            {
                messagesRepository.Delete(message);
            }
        }

        #endregion

    }
}
