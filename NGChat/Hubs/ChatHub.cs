using Microsoft.AspNet.SignalR;
using NGChat.DataAccess;
using NGChat.DataAccess.Models;
using NGChat.Infrastructure.Utils;
using NGChat.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace NGChat.Hubs
{
    public class ChatHub : Hub
    {
        public void SendMessage(string message)
        {
            ChatUser chatUser = null;

            if (Context.User != null && Context.User.Identity.IsAuthenticated)
            {
                using (var chatContext = new ChatContext())
                {
                    User user = chatContext.Users.FirstOrDefault(x => x.Name == Context.User.Identity.Name);

                    if (user != null)
                    {
                        user.LastActivity = DateTime.Now;
                        chatContext.Entry(user).State = EntityState.Modified;
                        chatContext.SaveChanges();

                        chatUser = new ChatUser()
                        {
                            Id = user.Id,
                            Name = user.Name
                        };
                    }
                }
            }

            message = WebUtility.HtmlEncode(message);

            message = message.Replace("\n", "<br />");
            message = message.Replace("\r\n", "<br />");

            ExpandUrlsParser urlsParser = new ExpandUrlsParser();
            urlsParser.Target = "_blank";
            message = urlsParser.ExpandUrls(message);

            EmoticonParser emoticonParser = new EmoticonParser();
            message = emoticonParser.Parse(message);

            if (chatUser != null)
                Clients.All.appendMessage(chatUser, message);
        }

        public override Task OnConnected()
        {
            if (Context.User != null && Context.User.Identity.IsAuthenticated)
            {
                using (var chatContext = new ChatContext())
                {
                    User user = chatContext.Users.FirstOrDefault(x => x.Name == Context.User.Identity.Name);

                    if (user != null)
                    {
                        user.LastActivity = DateTime.Now;
                        user.HubConnections.Add(new HubConnection()
                            {
                                ConnectionId = Context.ConnectionId
                            });
                        chatContext.Entry(user).State = EntityState.Modified;
                        chatContext.SaveChanges();

                        ChatUser chatUser = new ChatUser()
                        {
                            Id = user.Id,
                            Name = WebUtility.HtmlEncode(user.Name)
                        };

                        string[] userExistingConnections = user.HubConnections.Select(x => x.ConnectionId).ToArray();
                        Clients.AllExcept(userExistingConnections).userConnected(chatUser);
                    }
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            if (Context.User != null && Context.User.Identity.IsAuthenticated)
            {
                using (var chatContext = new ChatContext())
                {
                    User user = chatContext.Users.FirstOrDefault(x => x.Name == Context.User.Identity.Name);

                    if (user != null)
                    {
                        user.LastActivity = DateTime.Now;
                        var connection = user.HubConnections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

                        if (connection != null)
                            chatContext.Entry(connection).State = EntityState.Deleted;

                        chatContext.Entry(user).State = EntityState.Modified;
                        chatContext.SaveChanges();

                        ChatUser chatUser = new ChatUser()
                        {
                            Id = user.Id,
                            Name = WebUtility.HtmlEncode(user.Name)
                        };

                        string[] userExistingConnections = user.HubConnections.Select(x => x.ConnectionId).ToArray();
                        Clients.AllExcept(userExistingConnections).userDisconnected(chatUser);
                    }
                }
            }

            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }

}