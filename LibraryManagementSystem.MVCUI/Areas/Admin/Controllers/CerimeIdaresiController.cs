using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class CerimeIdaresiController : Controller
    {
        //private LibraryManagementSystemDBEntities db = new LibraryManagementSystemDBEntities();
        CerimeManager cerimeManager = new CerimeManager();
        IcareManager icareManager = new IcareManager();
        IstifadechiManager istifadechiManager = new IstifadechiManager();

        // GET: Admin/CerimeIdaresi
        public ActionResult IndexCerime(string searchText)
        {
            var cerime = cerimeManager.GetAllByInclude(c => c.Icare, c => c.Istifadechi);
            //var cerime = db.Cerime.Include(c => c.Icare).Include(c => c.Istifadechi);

            var cerim = cerimeManager.GetAll();
            if (!string.IsNullOrEmpty(searchText))
            {
                cerim = cerim
                    .Where(c =>
                        (c.Istifadechi != null && c.Istifadechi.Adi != null && c.Istifadechi.Adi.Contains(searchText)) ||
                        (c.Istifadechi != null && c.Istifadechi.Soyadi != null && c.Istifadechi.Soyadi.Contains(searchText))
                    ).ToList();
                return View(cerim);
            }

            return View(cerime);
        }

        public JsonResult GetIcareDetails(int icareId)
        {
            var icare = icareManager.GetAll()
                .Where(x => x.IcareID == icareId)
                .Select(x => new
                {
                    IstifadechiID = x.IstifadechiID,
                    IstifadechiAdSoyadi = x.Istifadechi.AdSoyadi,
                    KitabAdi = x.Kitab.KitabAdi,
                    IcareTarixi = x.IcareTarixi.ToString("yyyy-MM-dd"),
                    SonTarix = x.SonTarix.ToString("yyyy-MM-dd")
                })
                .FirstOrDefault();

            return Json(icare, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/CerimeIdaresi/DetailsCerime/5
        public ActionResult DetailsCerime(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cerime cerime = cerimeManager.FindById(id.Value);
            if (cerime == null)
            {
                return HttpNotFound();
            }
            return View(cerime);
        }

        // GET: Admin/CerimeIdaresi/CreateCerime
        public ActionResult CreateCerime()
        {
            ViewBag.IcareID = new SelectList(icareManager.GetAll(), "IcareID", "IcareID");
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi");

            ViewBag.StatusList = new SelectList(new List<string> { "Gecikir", "İtirilib" });

            return View(new Cerime());
        }

        // POST: Admin/CerimeIdaresi/CreateCerime
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCerime(Cerime cerime)
        {
            if (ModelState.IsValid)
            {
                cerimeManager.Add(cerime);
                return RedirectToAction("IndexCerime");
            }

            ViewBag.IcareID = new SelectList(icareManager.GetAll(), "IcareID", "IcareID", cerime.IcareID);
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", cerime.IstifadechiID);
            ViewBag.StatusList = new SelectList(new List<string> { "Gecikir", "İtirilib" });
            return View(cerime);
        }

        // GET: Admin/CerimeIdaresi/EditCerime/5
        public ActionResult EditCerime(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cerime cerime = cerimeManager.FindById(id.Value);
            if (cerime == null)
            {
                return HttpNotFound();
            }
            ViewBag.IcareID = new SelectList(icareManager.GetAll(), "IcareID", "IcareID", cerime.IcareID);
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", cerime.IstifadechiID);
            return View(cerime);
        }

        // POST: Admin/CerimeIdaresi/EditCerime/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCerime(Cerime cerime)
        {
            if (ModelState.IsValid)
            {
                cerimeManager.Update(cerime);
                return RedirectToAction("IndexCerime");
            }
            ViewBag.IcareID = new SelectList(icareManager.GetAll(), "IcareID", "IcareID", cerime.IcareID);
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", cerime.IstifadechiID);
            return View(cerime);
        }

        // GET: Admin/CerimeIdaresi/DeleteCerime/5
        public ActionResult DeleteCerime(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cerime cerime = cerimeManager.FindById(id.Value);
            if (cerime == null)
            {
                return HttpNotFound();
            }
            return View(cerime);
        }

        // POST: Admin/CerimeIdaresi/DeleteCerime/5
        [HttpPost, ActionName("DeleteCerime")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cerime cerime = cerimeManager.FindById(id);
            cerimeManager.Delete(id);
            return RedirectToAction("IndexCerime");
        }
    }
}
