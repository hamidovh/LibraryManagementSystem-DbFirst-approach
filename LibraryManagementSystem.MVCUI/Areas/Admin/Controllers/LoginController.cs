using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        IstifadechiManager istifadechiManager = new IstifadechiManager();
        RolManager rolManager = new RolManager();

        // GET: Admin/Login
        public ActionResult IndexLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexLogin(string email, string shifre, bool? rememberMe)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(shifre))
            {
                TempData["Message"] = "Email və ya şifrə boş buraxılmamalıdır!";
            }
            else
            {
                var istifadechi = istifadechiManager.GetAll().FirstOrDefault(i => i.Email == email && i.Shifre == shifre && i.Aktivdirmi == true && i.RolID == 1);
                var rol = rolManager.GetAll(r => r.RolAdi == "Admin");
                if (istifadechi != null) //rol = "Admin"
                {
                    Session["Admin"] = istifadechi;
                    Session["AdminID"] = istifadechi.IstifadechiID;
                    Session["AdminName"] = istifadechi.AdSoyadi;

                    // rememberMe = true olarsa cookie qalıcı olacaq:
                    FormsAuthentication.SetAuthCookie(istifadechi.AdSoyadi, rememberMe ?? false);

                    if (Request.QueryString["ReturnUrl"] == null) return Redirect("/Admin/Default");
                    else return Redirect(Request.QueryString["ReturnUrl"] ?? "/Admin/Default");
                    //return Redirect(Request.QueryString["ReturnUrl"]);

                    //return RedirectToAction("Index", "Default");
                }
                else
                {
                    TempData["Message"] = "Email və ya şifrə yalnışdır!";
                }
            }

            return View();
        }

        // GET: Admin/LogOut
        public ActionResult IndexLogout()
        {
            Session.Remove("Admin");
            FormsAuthentication.SignOut();

            return RedirectToAction("IndexLogin");
        }
    }
}
