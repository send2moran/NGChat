using NGChat.DataAccess;
using NGChat.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NGChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new ChatContext())
            {
                var inactiveUsers = context.Users.Where(x => x.LastActivity < EntityFunctions.AddMinutes(DateTime.Now, -60));

                foreach (var user in inactiveUsers)
                    context.Entry(user).State = EntityState.Deleted;

                context.SaveChanges();
            }

            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult Test()
        {
            Response.Write("HomeController Test start...");

            using (ChatContext context = new ChatContext())
            {
                User user = new User();
                user.Name = "Piotrek - " + Guid.NewGuid().ToString();
                user.LastActivity = DateTime.Now;
                user.HubConnections.Add(new HubConnection() { ConnectionId = Guid.NewGuid().ToString().Substring(2, 10) });
                user.HubConnections.Add(new HubConnection() { ConnectionId = Guid.NewGuid().ToString().Substring(2, 10) });
                context.Entry(user).State = EntityState.Added;
                context.SaveChanges();
            }

            using (ChatContext context = new ChatContext())
            {
                foreach (var user in context.Users)
                {
                    Response.Write("<br />" + user.Name);

                    if (user.HubConnections.Count() > 0)
                    {
                        Response.Write("<br />  User's hub connections (" + user.HubConnections.Count() + ")");
                        foreach (var conn in user.HubConnections)
                        {
                            Response.Write("<br />     - " + conn.ConnectionId);
                        }
                    }
                }
            }

            return new EmptyResult();
        }

        public ActionResult CopyMessageDialog()
        {
            return View();
        }
    }
}
