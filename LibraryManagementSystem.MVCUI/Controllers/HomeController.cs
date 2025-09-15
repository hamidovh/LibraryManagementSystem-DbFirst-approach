using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
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

        public ActionResult Index()
        {
            return View(kitabManager.GetAll());
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
    }
}
