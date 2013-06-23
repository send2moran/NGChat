using NGChat.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NGChat.Infrastructure.Validation
{
    public class UserExistsAttribute : ValidationAttribute
    {
        private bool _shouldExists = true;

        public UserExistsAttribute() 
            : base() { }

        public UserExistsAttribute(bool shouldExists) 
            : base()
        {
            _shouldExists = shouldExists;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            using (var context = new ChatContext())
            {
                string name = value.ToString();

                return _shouldExists == context.Users.Any(x => x.Name == name);
            }
        }
    }
}