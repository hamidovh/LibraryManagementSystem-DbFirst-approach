using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.BL;
using LibraryManagementSystem.MVCUI.Models;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class ElaqeController : Controller
    {
        ElaqeManager elaqeManager = new ElaqeManager();

        // GET: Admin/Elaqe
        public ActionResult IndexElaqe()
        {
            return View(elaqeManager.GetAll());
        }

        // GET: Admin/Elaqe/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Elaqe elaqe = db.Elaqes.Find(id);
        //    if (elaqe == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(elaqe);
        //}

        // GET: Admin/Elaqe/CreateElaqe
        public ActionResult CreateElaqe()
        {
            return View();
        }

        // POST: Admin/Elaqe/CreateElaqe
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateElaqe(Elaqe elaqe)
        {
            if (ModelState.IsValid)
            {
                elaqe.ElaqeTarixi = DateTime.Now;
                elaqeManager.Add(elaqe);
                return RedirectToAction("IndexElaqe");
            }

            return View(elaqe);
        }

        // GET: Admin/Elaqe/EditElaqe/5
        public ActionResult EditElaqe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elaqe elaqe = elaqeManager.FindById(id.Value);
            if (elaqe == null)
            {
                return HttpNotFound();
            }
            return View(elaqe);
        }

        // POST: Admin/Elaqe/EditElaqe/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditElaqe(Elaqe elaqe)
        {
            if (ModelState.IsValid)
            {
                elaqeManager.Update(elaqe);
                return RedirectToAction("IndexElaqe");
            }
            return View(elaqe);
        }

        // GET: Admin/Elaqe/DeleteElaqe/5
        public ActionResult DeleteElaqe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elaqe elaqe = elaqeManager.FindById(id.Value);
            if (elaqe == null)
            {
                return HttpNotFound();
            }
            return View(elaqe);
        }

        // POST: Admin/Elaqe/Delete/5
        [HttpPost, ActionName("DeleteElaqe")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Elaqe elaqe = elaqeManager.FindById(id);
            elaqeManager.Delete(elaqe.ElaqeID);
            return RedirectToAction("IndexElaqe");
        }
    }
}
