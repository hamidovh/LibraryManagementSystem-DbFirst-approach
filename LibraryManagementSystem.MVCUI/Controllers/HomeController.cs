using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.MVCUI.Models;
using LibraryManagementSystem.MVCUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Controllers
{
    public class HomeController : Controller
    {
        KitabManager kitabManager = new KitabManager();
        MuellifManager muellifManager = new MuellifManager();
        KateqoriyaManager kateqoriyaManager = new KateqoriyaManager();
        SliderManager sliderManager = new SliderManager();
        ElaqeManager elaqeManager = new ElaqeManager();

        public ActionResult Index()
        {
            var sehifeModeli = new EsasSehifeVM
            {
                Sliders = sliderManager.GetAll(),
                Kitablar = kitabManager.GetAllByIncludes(k => k.Muellif, k => k.Kateqoriya).ToList()
                //kitabManager.GetAll();
            };

            return View(sehifeModeli);
        }

        public PartialViewResult _PartialMenu()
        {
            var kateqoriyalar = kateqoriyaManager.GetAll();
            return PartialView(kateqoriyalar);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Haqqımızda səhifəsi.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Əlaqə səhifəsi.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(string email, string Adi, string Soyadi, string Mesaj)
        {
            try
            {
                var forMesaj = (new Elaqe
                {
                    Adi = Adi,
                    Soyadi = Soyadi,
                    ElaqeTarixi = DateTime.Now,
                    Email = email,
                    Mesaj = Mesaj
                });

                var emeliyyatNeticesi = elaqeManager.Add(forMesaj);

                //bool mailGonderildimi = MailHelper.SendMail(forMesaj);

                if (emeliyyatNeticesi > 0) //if (emeliyyatNeticesi > 0 && mailGonderildimi == true)
                {
                    TempData["Message"] = $"Hörmətli {Adi} {Soyadi}, Mesajınız Göndərildi!";
                }
            }
            catch (Exception)
            {
                TempData["Message"] = $"Xəta Baş Verdi! Hörmətli {Adi} {Soyadi}, Mesajınız Göndərilmədi!";
            }

            return View();
        }

        public ActionResult KitabDetal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kitab kitab = kitabManager.GetAllByIncludes(k => k.Muellif, k => k.Kateqoriya)
                .FirstOrDefault(k => k.KitabID == id.Value);
            if (kitab == null)
            {
                return HttpNotFound();
            }

            return View(kitab);
        }

        public ActionResult Kateqoriya(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var kitablar = kitabManager.GetAllByIncludes(k => k.Kateqoriya).Where(k => k.Kateqoriya.Any(c => c.KateqoriyaID == id.Value)).ToList();

            if (!kitablar.Any())
            {
                return HttpNotFound();
            }

            return View(kitablar);
        }
    }
}
