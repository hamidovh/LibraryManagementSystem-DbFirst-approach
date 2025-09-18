using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.MVCUI.Areas.User.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryManagementSystem.MVCUI.Areas.User.Controllers
{
    // User Area AccountController.cs (~/Areas/User/Controllers/AccountController):
    public class AccountController : Controller
    {
        IstifadechiManager istifadechiManager = new IstifadechiManager();

        // GET: User/Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: User/Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                Istifadechi yeniIstifadechi = new Istifadechi
                {
                    Adi = model.Adi,
                    Soyadi = model.Soyadi,
                    DoghumTarixi = model.DoghumTarixi,
                    Cins = model.Cins,
                    FinKod = model.FinKod,
                    Email = model.Email,
                    TelefonNo = model.TelefonNo,
                    Adres = model.Adres,
                    IstifadechiAdi = model.IstifadechiAdi,
                    Shifre = model.Shifre, // gələcəkdə hash-lənməlidir
                    Aktivdirmi = true,
                    QeydiyyatTarixi = DateTime.Now,
                    RolID = 2 // 1 = Admin, 2 = İstifadəçi
                };

                istifadechiManager.Add(yeniIstifadechi);

                TempData["SuccessMessage"] = "Qeydiyyat uğurla tamamlandı!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Xəta baş verdi: " + ex.Message);
                return View(model);
            }
        }

        // GET: User/Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string shifre, bool? rememberMe)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(shifre))
            {
                TempData["Message"] = "Email və ya şifrə boş buraxılmamalıdır!";
            }
            else
            {
                var istifadechi = istifadechiManager.GetAll()
                    .FirstOrDefault(i => i.Email == email && i.Shifre == shifre && i.Aktivdirmi == true && i.RolID == 2); // RolID = 2 (İstifadəçi)

                if (istifadechi != null)
                {
                    Session["User"] = istifadechi;
                    Session["UserID"] = istifadechi.IstifadechiID;
                    Session["UserName"] = istifadechi.AdSoyadi;

                    FormsAuthentication.SetAuthCookie(istifadechi.AdSoyadi, rememberMe ?? false);

                    if (Request.QueryString["ReturnUrl"] != null)
                        return Redirect(Request.QueryString["ReturnUrl"]);
                    else
                        return RedirectToAction("Index", "Home", new { area = "User" }); // User Areasının əsas səhifəsi
                }
                else
                {
                    TempData["Message"] = "Email və ya şifrə yalnışdır!";
                }
            }

            return View();
        }

        // GET: User/Account/Logout
        public ActionResult Logout()
        {
            Session.Remove("User");
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}
