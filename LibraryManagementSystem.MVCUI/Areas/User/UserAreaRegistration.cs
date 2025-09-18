using System.Security.Policy;
using System.Web.Mvc;
using System.Xml.Linq;

namespace LibraryManagementSystem.MVCUI.Areas.User
{
    public class UserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "User_default",
                "User/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "LibraryManagementSystem.MVCUI.Areas.User.Controllers" } // namespaces parametri MVC-yə göstərir ki, bu area üçün hansı namespace-də controller axtarsın
            );
        }
    }
}
/*
MVC routing-də namespace konflikti ilə bağlı xəta yarana bilər. Layihədə həm LibraryManagementSystem.MVCUI.Controllers.HomeController, həm də LibraryManagementSystem.MVCUI.Areas.User.Controllers.HomeController var. Standart route { controller}/{ action}/{ id} istifadə ediləndə ASP.NET MVC hansı HomeController-ı çağıracağını bilmir (Multiple types found erroru). Bu xətanın yaranmaması üçün Areas route-larına namespaces parametri əlavə edilməlidir.
*/
