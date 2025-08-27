using LibraryManagementSystem.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        IstifadechiManager istifadechiManager = new IstifadechiManager();

        // GET: Admin/Login
        public ActionResult IndexLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexLogin(string IstifadechiAdi, string Shifre)
        {
            try
            {
                var istifadechi = istifadechiManager.Get(i => i.IstifadechiAdi == IstifadechiAdi && i.Shifre == Shifre && i.Aktivdirmi == true);
                if (istifadechi != null)
                {
                    Session["admin"] = istifadechi;
                    return Redirect("/Admin");
                }
                else
                {
                    TempData["mesaj"] = "İstifadəçi adı və ya şifrə yalnışdır!";
                }
            }
            catch (Exception)
            {
                TempData["mesaj"] = "Xəta baş verdi!";
            }

            return View();
        }

        public ActionResult LogOut()
        {
            Session.Remove("admin");
            return Redirect("/Admin/Login");
        }
    }
}
