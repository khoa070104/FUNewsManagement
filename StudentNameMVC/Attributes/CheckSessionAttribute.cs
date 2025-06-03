using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentNameMVC.Attributes
{
    public class CheckSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var accountRole = context.HttpContext.Session.GetString("AccountRole");
            var accountEmail = context.HttpContext.Session.GetString("AccountEmail");

            // Nếu chưa đăng nhập và không phải đang ở trang Login
            if (string.IsNullOrEmpty(accountRole) || string.IsNullOrEmpty(accountEmail))
            {
                var controller = context.Controller as Controller;
                if (controller != null)
                {
                    context.Result = controller.RedirectToAction("Login", "Account");
                }
            }

            base.OnActionExecuting(context);
        }
    }
} 