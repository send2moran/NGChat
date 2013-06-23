using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NGChat.ViewModels.User
{
    public class ChatUserVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}