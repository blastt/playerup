using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.Hubs
{
    public class MessageHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
        private void UpdateMessage(int messagesCounter, string userId)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            // отправляем сообщение
            context.Clients.User(userId).updateMessage(messagesCounter);

            //context.Clients. User(User.Identity.GetUserId()).updateMessage(messagesCounter);
        }

        private void UpdateMessageInDialog(int messagesCounter, string lastMessage, string date, int dialogId, string userName, string companionId, string companionName)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            // отправляем сообщение
            context.Clients.User(userName).updateMessageInDialog(userName, companionId, companionName, messagesCounter, lastMessage, date, dialogId);

            //context.Clients. User(User.Identity.GetUserId()).updateMessage(messagesCounter);
        }

        private void AddMessage(string senderId, string receiverName, string senderName, string messageBody, string date)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            // отправляем сообщение
            //context.Clients.All.addMessage(senderId, receiverId, senderName, messageBody, date);
            context.Clients.User(senderName).addMessage(senderId, receiverName, senderName, messageBody, date);
            context.Clients.User(receiverName).addMessage(senderId, receiverName, senderName, messageBody, date);
            //context.Clients. User(User.Identity.GetUserId()).updateMessage(messagesCounter);
        }
    }
}