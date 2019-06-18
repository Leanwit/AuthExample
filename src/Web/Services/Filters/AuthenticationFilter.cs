namespace Web.Services.Filters
{
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;

    public class RoleAuthenticateFilter : ActionFilterAttribute
    {
        public string Page { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _roleAuthenticateService = (RoleAuthenticateService) context.HttpContext.RequestServices.GetService(typeof(RoleAuthenticateService));

            var statusCode = _roleAuthenticateService.Execute(Page, context.HttpContext).Result;

            if (statusCode == HttpStatusCode.Forbidden)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                var redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "AccessDenied");
                redirectTargetDictionary.Add("controller", "Account");
                context.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }

            if (statusCode == HttpStatusCode.BadRequest || statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.NotFound)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                var redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "Index");
                redirectTargetDictionary.Add("controller", "Account");
                redirectTargetDictionary.Add("returnUrl", context.HttpContext.Request.Path);

                context.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }

            if (statusCode == HttpStatusCode.OK) context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            base.OnActionExecuting(context);
        }
    }
}