using NGChat.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NGChat.ViewModels.User
{
    public class LoginUserVM
    {
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [MaxLength(255, ErrorMessage = "Nazwa użytkownika nie może być dłuższa niż {0} znaków.")]
        [IsAuthorized(false, ErrorMessage = "Użytkownik jest już zalogowany.")]
        [UserExists(false, ErrorMessage = "Nazwa użytkownika jest zajęta.")]
        public string Username { get; set; }
    }
}