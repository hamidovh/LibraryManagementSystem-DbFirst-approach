using System;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.User.Controllers
{
    // User Area UserController.cs (~/Areas/User/Controllers/UserController):
    public class UserController : Controller
    {
        // İstifadəçi əsas səhifəsi (dashboard və ya yönləndirmə üçün):
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Account", new { area = "User" });

            return RedirectToAction("MyProfile"); // Default olaraq profilə yönləndiririk
        }

        // Mənim Hesabım (Profile səhifəsi):
        public ActionResult MyProfile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Account", new { area = "User" });

            return View();
        }

        // Çıxış etmək (Logout):
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            System.Web.Security.FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account", new { area = "User" });
        }
    }
}
