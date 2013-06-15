using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NGChat.Models
{
    public class AjaxResult
    {
        public AjaxResult()
        {
            Errors = new List<string>();
        }

        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public List<string> Errors { get; set; }
    }
}