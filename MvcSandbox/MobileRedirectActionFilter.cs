using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;

namespace MvcSandbox
{
    public class MobileRedirectActionFilter : Attribute, IActionFilter
    {
        public string Action { get; set; }
        public string Controller { get; set; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("Action Executed");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // to z jakiegoś powodu nie działa
            if (context.HttpContext.Request.Headers.Keys.Contains("x-mobile"))
            {
                context.Result = new RedirectToActionResult(Action, Controller, null);
            }
        }
    }
}