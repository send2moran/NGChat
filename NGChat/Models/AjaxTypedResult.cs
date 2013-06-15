using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NGChat.Models
{
    public class AjaxTypedResult<T> : AjaxResult where T : class
    {
        public AjaxTypedResult() : base()
        {

        }

        public T Model { get; set; }
    }
}