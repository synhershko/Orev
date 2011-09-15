using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orev.Helpers
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class AjaxOnlyAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var request = filterContext.HttpContext.Request;
			if (!request.IsAjaxRequest())
				filterContext.Result = new HttpNotFoundResult("Only Ajax calls are permitted.");
		}
	}
}