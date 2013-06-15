using NGChat.Models;
using NGChat.DataAccess;
using NGChat.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NGChat.Infrastructure.Extensions;
using System.Web.Security;
using System.Data;
using System.Data.Objects;
using System.Net;

namespace NGChat.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Create(ChatUser model)
        {
            AjaxTypedResult<ChatUser> result = new AjaxTypedResult<ChatUser>();

            if (!User.Identity.IsAuthenticated && ModelState.IsValid)
            {
                using (var context = new ChatContext())
                {
                    User user = context.Users.FirstOrDefault(x => x.Name == model.Name);

                    if (user == null)
                    {
                        User newUser = new User()
                        {
                            Name = model.Name,
                            LastActivity = DateTime.Now
                        };
                        context.Entry(newUser).State = EntityState.Added;
                        context.SaveChanges();

                        result.Model = new ChatUser()
                        {
                            Name = newUser.Name,
                            Id = newUser.Id
                        };
                        result.Success = true;
                        result.SuccessMessage = "Użytkownik został dodany.";
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                    }
                    else
                        result.Errors.Add("Wystąpił błąd. Nazwa użytkownika jest zajęta.");
                }
            }
            else
                result.Errors.Add("Wystąpił błąd. Użytkownik jest już zalogowany lub nazwa jest niepoprawna.");

            return this.JsonCamelCase(result);
        }

        public ActionResult FindPreviousSession()
        {
            AjaxTypedResult<ChatUser> result = new AjaxTypedResult<ChatUser>();

            if (User.Identity.IsAuthenticated)
            {
                using (var context = new ChatContext())
                {
                    User user = context.Users.FirstOrDefault(x => x.Name == User.Identity.Name);

                    if (user != null)
                    {
                        result.Model = new ChatUser()
                        {
                            Name = user.Name,
                            Id = user.Id
                        };
                        result.Success = true;

                        user.LastActivity = DateTime.Now;
                        context.Entry(user).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
            }

            return this.JsonCamelCase(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete()
        {
            AjaxResult result = new AjaxResult();

            if (User.Identity.IsAuthenticated)
            {
                using (var context = new ChatContext())
                {
                    User user = context.Users.FirstOrDefault(x => x.Name == User.Identity.Name);

                    if (user != null)
                    {
                        context.Entry(user).State = EntityState.Deleted;
                        context.SaveChanges();

                        result.Success = true;
                        FormsAuthentication.SignOut();
                    }
                }
            }

            return this.JsonCamelCase(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckConnectedUsers()
        {
            AjaxTypedResult<List<ChatUser>> result = new AjaxTypedResult<List<ChatUser>>();

            if (User.Identity.IsAuthenticated)
            {
                using (var chatContext = new ChatContext())
                {
                    List<ChatUser> chatUsers = new List<ChatUser>();
                    // pobiera userow z aktyanoscia nie starsza niz 5 minut
                    // niestety sql server compact nie posiada funkcji EntityFunctions.AddMinutes ;/
                    var users = chatContext.Users.Where(x => x.LastActivity >= EntityFunctions.AddMinutes(DateTime.Now, -5));
                    //var users = chatContext.Users.Where(x => x.Name != User.Identity.Name);

                    foreach (var user in users)
                    {
                        chatUsers.Add(new ChatUser()
                        {
                            Id = user.Id,
                            Name = user.Name
                        });
                    }

                    result.Model = chatUsers;
                    result.Success = true;
                }
            }

            return this.JsonCamelCase(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckIfLogged()
        {
            AjaxTypedResult<ChatUser> result = new AjaxTypedResult<ChatUser>();

            if (User.Identity.IsAuthenticated)
            {
                using (var context = new ChatContext())
                {
                    User user = context.Users.FirstOrDefault(x => x.Name == User.Identity.Name);

                    if (user != null)
                    {
                        result.Model = new ChatUser()
                        {
                            Name = user.Name,
                            Id = user.Id
                        };
                        result.Success = true;

                        user.LastActivity = DateTime.Now;
                        context.Entry(user).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
            }

            return this.JsonCamelCase(result, JsonRequestBehavior.AllowGet);
        }
    }
}
