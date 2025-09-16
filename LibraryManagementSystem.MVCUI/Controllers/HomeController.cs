using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.MVCUI.Models;
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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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
