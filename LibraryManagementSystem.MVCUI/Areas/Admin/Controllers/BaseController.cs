using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Admin session yoxlanışı:
            if (Session["Admin"] == null)
            {
                // Əgər login olunmayıbsa, IndexLogin səhifəsinə yönləndir:
                filterContext.Result = new RedirectResult("/Admin/Login/IndexLogin");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
