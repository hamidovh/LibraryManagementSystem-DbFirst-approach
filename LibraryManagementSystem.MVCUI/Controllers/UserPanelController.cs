using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.MVCUI.Models;
using System.Linq;
using System.Web.Mvc;
using WebGrease;

namespace LibraryManagementSystem.MVCUI.Controllers
{
    [Authorize] // sadəcə login olmuş istifadəçi görə bilsin
    public class UserPanelController : Controller
    {
        private IcareManager icareManager = new IcareManager();
        private CerimeManager cerimeManager = new CerimeManager();

        public ActionResult IndexUser()
        {
        var userID = (int)Session["UserID"];

        var icare = icareManager.GetAll()
            .Where(x => x.IstifadechiID == userID)
            .ToList();

        var cerime = cerimeManager.GetAll()
            .Where(x => x.IstifadechiID == userID)
            .ToList();

        var model = new UserPanelViewModel
        {
            Icare = icare,
            Cerime = cerime,
            AktivIcareSayi = icare.Count(x => x.QaytarilmaTarixi == null)
        };

        return View(model);
        }

        // İstifadəçinin bütün icarələri:
        public ActionResult MenimIcarelerim()
        {
            var userID = (int)Session["UserID"]; // login zamanı saxlanılır
            var icare = icareManager.GetAll()
                .Where(x => x.IstifadechiID == userID)
                .ToList();

            return View(icare);
        }

        // İstifadəçinin cərimələri:
        public ActionResult MenimCerimelerim()
        {
            var userID = (int)Session["UserID"];
            var cerime = cerimeManager.GetAll()
                .Where(x => x.IstifadechiID == userID)
                .ToList();

            return View(cerime);
        }

        // Yeni kitab icarəsi (limit yoxlanacaq):
        [HttpGet]
        public ActionResult IcareKitab()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IcareKitab(int kitabID)
        {
            var userID = (int)Session["UserID"];

            // Aktiv icarələrin sayını yoxlayırıq:
            var activeIcareler = icareManager.GetAll()
                .Count(x => x.IstifadechiID == userID && x.QaytarilmaTarixi == null);

            if (activeIcareler >= 5)
            {
                TempData["Error"] = "Maksimal icarə limiti (5 kitab) dolub!";
                return RedirectToAction("MenimIcarelerim");
            }

            // Əks halda icarə əlavə edilir:
            icareManager.Add(new Icare
            {
                IstifadechiID = userID,
                KitabID = kitabID,
                IcareTarixi = System.DateTime.Now,
                QaytarilmaTarixi = null
            });

            return RedirectToAction("MenimIcarelerim");
        }
    }
}
