using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.MVCUI.Areas.User.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.User.Controllers
{
    // User Area UserController.cs (~/Areas/User/Controllers/UserController):
    public class UserController : Controller
    {
        IstifadechiManager istifadechiManager = new IstifadechiManager();
        KitabManager kitabManager = new KitabManager();
        IcareManager icareManager = new IcareManager();
        CerimeManager cerimeManager = new CerimeManager();

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

            var user = (Istifadechi)Session["User"];

            var model = new RegisterVM
            {
                Adi = user.Adi,
                Soyadi = user.Soyadi,
                DoghumTarixi = user.DoghumTarixi.Value,
                Cins = user.Cins,
                FinKod = user.FinKod,
                Email = user.Email,
                TelefonNo = user.TelefonNo,
                Adres = user.Adres,
                IstifadechiAdi = user.IstifadechiAdi,
                Shifre = user.Shifre
            };

            return View(model);
        }

        // GET: User/EditProfile
        [HttpGet]
        public ActionResult EditProfile()
        {
            var user = (Istifadechi)Session["User"];
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });

            // Repository vasitəsilə user gətiririk:
            //var user = istifadechiManager.Get(u => u.IstifadechiAdi == username);

            if (user == null)
                return HttpNotFound();

            var model = new RegisterVM
            {
                Adi = user.Adi,
                Soyadi = user.Soyadi,
                DoghumTarixi = user.DoghumTarixi, // Nullable DateTime üçün yoxlama: DateTime.Value or DoghumTarixi = user.DoghumTarixi ?? DateTime.MinValue,
                Cins = user.Cins,
                FinKod = user.FinKod,
                Email = user.Email,
                TelefonNo = user.TelefonNo,
                Adres = user.Adres,
                IstifadechiAdi = user.IstifadechiAdi,
                Shifre = user.Shifre
                // Şifrəni burada göstərməyə ehtiyac varmı?
            };

            return View(model);
        }

        // POST: User/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(RegisterVM model)
        {
            // Model validasiyası zamanı problem yaranarsa xətaları TempData-ya at:
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .Where(s => !string.IsNullOrEmpty(s))
                                              .ToList();
                TempData["ModelErrors"] = string.Join(" | ", errors);
                return View(model);
            }

            var sessionUser = (Istifadechi)Session["User"];
            if (sessionUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });

            try
            {
                // DB-dən mövcud istifadəçini gətir:
                var userFromDb = istifadechiManager.FindById(sessionUser.IstifadechiID);
                if (userFromDb == null)
                    return HttpNotFound();

                // Yalnız scalar sahələri kopyala:
                userFromDb.Adi = model.Adi;
                userFromDb.Soyadi = model.Soyadi;
                userFromDb.DoghumTarixi = model.DoghumTarixi;
                userFromDb.Cins = model.Cins;
                userFromDb.FinKod = model.FinKod;
                userFromDb.Email = model.Email;
                userFromDb.TelefonNo = model.TelefonNo;
                userFromDb.Adres = model.Adres;
                userFromDb.IstifadechiAdi = model.IstifadechiAdi;
                userFromDb.Shifre = model.Shifre;

                // Şifrəni yalnız istifadəçi yeni şifrə daxil etmişsə yenilə:
                if (!string.IsNullOrWhiteSpace(model.Shifre))
                {
                    userFromDb.Shifre = model.Shifre;
                }

                // Repository-dəki Redakte metodu:
                istifadechiManager.Redakte(userFromDb, userFromDb.IstifadechiID);

                // Session-ı yenilə ki, UI yeni məlumatları göstərsin:
                Session["User"] = userFromDb;

                TempData["SuccessMessage"] = "Profil məlumatlarınız uğurla yeniləndi!";
                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Xəta baş verdi: " + ex.Message);
                return View(model);
            }
        }

        // GET: User/Icarelerim
        public ActionResult Icarelerim()
        {
            var sessionUser = (Istifadechi)Session["User"];
            if (sessionUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });

            int userId = sessionUser.IstifadechiID;

            // İstifadəçinin bütün icarələrini gətiririk:
            //var icare = icareManager.GetAllByInclude(i => i.Kitab);

            var icareler = icareManager.GetAllByInclude(i => i.Kitab)
                               .Where(i => i.IstifadechiID == userId)
                               .OrderByDescending(i => i.IcareTarixi)
                               .Select(i => new IcareVM
                               {
                                   IcareID = i.IcareID,
                                   KitabAdi = i.Kitab != null ? i.Kitab.KitabAdi : "",
                                   IcareTarixi = i.IcareTarixi,
                                   SonTarix = i.SonTarix,
                                   Qaytarilibmi = i.Qaytarilibmi,
                                   QaytarilmaTarixi = i.QaytarilmaTarixi,
                                   Statusu = i.Statusu
                               }).ToList();

            return View(icareler);
        }

        // GET: User/Cerimelerim
        public ActionResult Cerimelerim()
        {
            var sessionUser = (Istifadechi)Session["User"];
            if (sessionUser == null)
                return RedirectToAction("Login", "Account", new { area = "User" });

            int userId = sessionUser.IstifadechiID;

            // İstifadəçinin bütün cərimələrini gətiririk:
            var cerimelerFromDb = cerimeManager.GetAllByInclude(c => c.Icare)
                           .Where(c => c.IstifadechiID == userId)
                           .OrderByDescending(c => c.CerimeTarixi)
                           .ToList();

            var cerimeler = cerimelerFromDb.Select(c => new CerimeVM
            {
                CerimeID = c.CerimeID,
                KitabAdi = c.Icare?.Kitab?.KitabAdi ?? "",
                HesablanmisMebleg = c.HesablanmisMebleg, 
                Odenilibmi = c.Odenilibmi,
                CerimeTarixi = c.CerimeTarixi,
                OdenmeTarixi = c.OdenmeTarixi,
                Sebeb = c.Sebeb
            }).ToList();

            return View(cerimeler);
        }

        // Çıxmaq (Logout):
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            System.Web.Security.FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account", new { area = "User" });
        }
    }
}
