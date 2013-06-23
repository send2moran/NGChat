using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NGChat.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            string pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" +
                             "@" +
                             @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            return Regex.IsMatch(email, pattern);
        }

        public static string ToCamelCase(this string text)
        {
            string output = "";

            if (!String.IsNullOrEmpty(text))
            {
                output = String.Format("{0}{1}",
                    text[0].ToString().ToLower(),
                    text.Substring(1, text.Length - 1));
            }

            return output;
        }
    }
}