using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NGChat.Models
{
    public class ChatUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa użytkownika nie może być pusta.")]
        public string Name { get; set; }
    }
}