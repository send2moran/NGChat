using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NGChat.Infrastructure.Validation
{
    public class IsAuthorizedAttribute : ValidationAttribute
    {
        private bool _shouldBeAuthorized = true;

        public IsAuthorizedAttribute() : base() { }

        public IsAuthorizedAttribute(bool shouldBeAuthorized)
            : base() 
        {
            _shouldBeAuthorized = shouldBeAuthorized;
        }

        public override bool IsValid(object value)
        {
            return _shouldBeAuthorized == HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}