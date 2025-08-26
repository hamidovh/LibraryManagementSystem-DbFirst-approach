using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.MVCUI.Models;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class RolIdaresiController : Controller
    {
        //private LibraryManagementSystemDBEntities db = new LibraryManagementSystemDBEntities();
        RolManager rolManager = new RolManager();

        // GET: Admin/RolIdaresi
        public ActionResult IndexRol()
        {
            return View(rolManager.GetAll());
        }

        // GET: Admin/RolIdaresi/DetailsRol/5
        public ActionResult DetailsRol(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = rolManager.FindById(id.Value);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        // GET: Admin/RolIdaresi/CreateRol
        public ActionResult CreateRol()
        {
            return View();
        }

        // POST: Admin/RolIdaresi/CreateRol
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRol([Bind(Include = "RolID,RolAdi")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                rolManager.Add(rol);
                return RedirectToAction("IndexRol");
            }

            return View(rol);
        }

        // GET: Admin/RolIdaresi/EditRol/5
        public ActionResult EditRol(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = rolManager.FindById(id.Value);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        // POST: Admin/RolIdaresi/EditRol/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRol([Bind(Include = "RolID,RolAdi")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                rolManager.Update(rol);
                return RedirectToAction("IndexRol");
            }
            return View(rol);
        }

        // GET: Admin/RolIdaresi/DeleteRol/5
        public ActionResult DeleteRol(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = rolManager.FindById(id.Value);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        // POST: Admin/RolIdaresi/DeleteRol/5
        [HttpPost, ActionName("DeleteRol")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rol rol = rolManager.FindById(id);
            rolManager.Delete(id);
            return RedirectToAction("IndexRol");
        }
    }
}
