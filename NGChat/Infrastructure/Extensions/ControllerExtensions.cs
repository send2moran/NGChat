using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NGChat.Infrastructure.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NGChat.Infrastructure.Extensions
{
    public static class ControllerExtensions
    {
        public static JsonNetResult JsonCamelCase(this Controller controller, object data)
        {
            return new JsonNetResult()
            {
                Data = data
            };
        }

        public static JsonNetResult JsonCamelCase(this Controller controller, object data, JsonRequestBehavior jsonBehavior)
        {
            return new JsonNetResult()
            {
                Data = data,
                JsonRequestBehavior = jsonBehavior
            };
        }
    }
}