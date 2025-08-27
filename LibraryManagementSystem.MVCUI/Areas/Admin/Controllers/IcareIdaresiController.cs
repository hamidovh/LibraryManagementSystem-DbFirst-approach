using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.MVCUI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebGrease;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class IcareIdaresiController : Controller
    {
        //private LibraryManagementSystemDBEntities db = new LibraryManagementSystemDBEntities();
        IcareManager icareManager = new IcareManager();
        IstifadechiManager istifadechiManager = new IstifadechiManager();
        KitabManager kitabManager = new KitabManager();

        // GET: Admin/IcareIdaresi
        public ActionResult IndexIcare()
        {
            var icare = icareManager.GetAllByInclude(i => i.Istifadechi, i => i.Kitab);
            //var icare = icareManager.GetAllByInclude("Istifadechi"); //icareManager.Include(i => i.Istifadechi).Include(i => i.Kitab);
            //var icare = icareManager.GetAllByInclude("Kitab");

            return View(icareManager.GetAll());
        }

        // GET: Admin/IcareIdaresi/DetailsIcare/5
        public ActionResult DetailsIcare(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Icare icare = icareManager.FindById(id.Value);
            if (icare == null)
            {
                return HttpNotFound();
            }
            return View(icare);
        }

        // GET: Admin/IcareIdaresi/CreateIcare
        public ActionResult CreateIcare()
        {
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi");
            ViewBag.KitabID = new SelectList(kitabManager.GetAll(), "KitabID", "KitabAdi");
            return View();
        }

        // POST: Admin/IcareIdaresi/CreateIcare
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIcare(Icare icare)
        {
            if (ModelState.IsValid)
            {
                icareManager.Add(icare);
                return RedirectToAction("IndexIcare");
            }

            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", icare.IstifadechiID);
            ViewBag.KitabID = new SelectList(kitabManager.GetAll(), "KitabID", "KitabAdi", icare.KitabID);
            return View(icare);
        }

        // GET: Admin/IcareIdaresi/EditIcare/5
        public ActionResult EditIcare(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Icare icare = icareManager.FindById(id.Value);
            if (icare == null)
            {
                return HttpNotFound();
            }
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", icare.IstifadechiID);
            ViewBag.KitabID = new SelectList(kitabManager.GetAll(), "KitabID", "KitabAdi", icare.KitabID);
            return View(icare);
        }

        // POST: Admin/IcareIdaresi/EditIcare/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIcare(Icare icare)
        {
            if (ModelState.IsValid)
            {
                icareManager.Update(icare);
                return RedirectToAction("IndexIcare");
            }
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", icare.IstifadechiID);
            ViewBag.KitabID = new SelectList(kitabManager.GetAll(), "KitabID", "KitabAdi", icare.KitabID);
            return View(icare);
        }

        // GET: Admin/IcareIdaresi/DeleteIcare/5
        public ActionResult DeleteIcare(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Icare icare = icareManager.FindById(id.Value);
            if (icare == null)
            {
                return HttpNotFound();
            }
            return View(icare);
        }

        // POST: Admin/IcareIdaresi/DeleteIcare/5
        [HttpPost, ActionName("DeleteIcare")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Icare icare = icareManager.FindById(id);
            icareManager.Delete(id);
            return RedirectToAction("IndexIcare");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
