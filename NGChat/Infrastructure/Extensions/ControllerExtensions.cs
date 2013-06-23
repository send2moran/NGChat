using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NGChat.Infrastructure.ActionResults;
using NGChat.ViewModels.Shared;
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

        public static List<AjaxError> GetErrorsForAjaxResult(this ModelStateDictionary modelState)
        {
            return (from ms in modelState
                               where ms.Value.Errors.Any()
                               select new AjaxError(
                                   String.IsNullOrEmpty(ms.Key) ? "" : ms.Key.ToCamelCase(),
                                   ms.Value.Errors.Select(e => e.ErrorMessage).ToList())
                                   ).ToList();
        }
    }
}