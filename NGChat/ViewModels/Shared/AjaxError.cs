using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NGChat.ViewModels.Shared
{
    public class AjaxError
    {
        public AjaxError(string error)
        {
            Messages = new List<string>();
            Messages.Add(error);
        }

        public AjaxError(List<string> errors)
        {
            Messages = errors;
        }

        public AjaxError(string key, string error)
        {
            Key = key;
            Messages = new List<string>();
            Messages.Add(error);
        }

        public AjaxError(string key, List<string> errors)
        {
            Key = key;
            Messages = errors;
        }

        public string Key { get; set; }
        public List<string> Messages { get; set; }
    }
}