using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NGChat.ViewModels.Shared
{
    public class AjaxResult
    {
        public AjaxResult()
        {
            Errors = new List<AjaxError>();
        }

        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public List<AjaxError> Errors { get; set; }
    }
}