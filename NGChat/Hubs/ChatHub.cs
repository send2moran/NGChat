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
        //private static readonly ConcurrentDictionary<string, HubUser> Users = new ConcurrentDictionary<string, HubUser>();

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

            //Clients.Others.addNewMessageToPage(name, message);
        }

        public override Task OnConnected()
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            //var user = Users.GetOrAdd(userName, _ => new HubUser
            //{
            //    Name = userName,
            //    ConnectionIds = new List<string>()
            //});

            //lock (user.ConnectionIds)
            //{
            //    user.ConnectionIds.Add(connectionId);

            //    // TODO: Broadcast the connected user
            //    Clients.AllExcept(user.ConnectionIds.ToArray()).userConnected(userName);
            //}

            Clients.Others.userConnected(WebUtility.HtmlEncode(userName));

            return base.OnConnected();
        }

        // dorobic disconnect po stronie klienta jak ktos kliknie logout
        public override Task OnDisconnected()
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            //HubUser user;
            //Users.TryGetValue(userName, out user);

            //if (user != null)
            //{

            //    lock (user.ConnectionIds)
            //    {
            //        user.ConnectionIds.RemoveAll(cid => cid.Equals(connectionId));

            //        if (!user.ConnectionIds.Any())
            //        {

            //            HubUser removedUser;
            //            Users.TryRemove(userName, out removedUser);

            //            // You might want to only broadcast this info if this 
            //            // is the last connection of the user and the user actual is 
            //            // now disconnected from all connections.
            //            Clients.Others.userDisconnected(userName);
            //        }
            //    }
            //}

            Clients.Others.userDisconnected(WebUtility.HtmlEncode(userName));

            return base.OnDisconnected();
        }
    }

    public class HubUser
    {
        public string Name { get; set; }
        public List<string> ConnectionIds { get; set; }
    }
}