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
using NGChat.ViewModels.User;
using NGChat.ViewModels.Shared;

namespace NGChat.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login(LoginUserVM model)
        {
            AjaxTypedResult<ChatUserVM> result = new AjaxTypedResult<ChatUserVM>();

            if (ModelState.IsValid)
            {
                using (var context = new ChatContext())
                {
                    User newUser = new User()
                    {
                        Name = model.Username,
                        LastActivity = DateTime.Now
                    };
                    context.Entry(newUser).State = EntityState.Added;
                    context.SaveChanges();

                    result.Model = new ChatUserVM()
                    {
                        Name = newUser.Name,
                        Id = newUser.Id
                    };

                    result.Success = true;
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                }
            }
            else
                result.Errors = ModelState.GetErrorsForAjaxResult();

            return this.JsonCamelCase(result);
        }

        public ActionResult FindPreviousSession()
        {
            AjaxTypedResult<ChatUserVM> result = new AjaxTypedResult<ChatUserVM>();

            if (User.Identity.IsAuthenticated)
            {
                using (var context = new ChatContext())
                {
                    User user = context.Users.FirstOrDefault(x => x.Name == User.Identity.Name);

                    if (user != null)
                    {
                        result.Model = new ChatUserVM()
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

        public ActionResult Logout()
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
            AjaxTypedResult<List<ChatUserVM>> result = new AjaxTypedResult<List<ChatUserVM>>();

            if (User.Identity.IsAuthenticated)
            {
                using (var chatContext = new ChatContext())
                {
                    List<ChatUserVM> chatUsers = new List<ChatUserVM>();
                    var users = chatContext.Users.Where(x => 
                        x.LastActivity >= EntityFunctions.AddMinutes(DateTime.Now, -30) &&
                        x.HubConnections.Count > 0);

                    foreach (var user in users)
                    {
                        chatUsers.Add(new ChatUserVM()
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
            AjaxTypedResult<ChatUserVM> result = new AjaxTypedResult<ChatUserVM>();

            if (User.Identity.IsAuthenticated)
            {
                using (var context = new ChatContext())
                {
                    User user = context.Users.FirstOrDefault(x => x.Name == User.Identity.Name);

                    if (user != null)
                    {
                        result.Model = new ChatUserVM()
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
