using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class IstifadechiIdaresiController : Controller
    {
        IstifadechiManager istifadechiManager = new IstifadechiManager();
        RolManager rolManager = new RolManager();

        // GET: Admin/IstifadechiIdaresi
        public ActionResult IndexIstifadechi()
        {
            return View(istifadechiManager.GetAll());
        }

        // GET: Admin/IstifadechiIdaresi/CreateIstifadechi
        public ActionResult CreateIstifadechi()
        {
            ViewBag.RolID = new SelectList(rolManager.GetAll(), "RolID", "RolAdi");

            return View();
        }

        // POST: Admin/IstifadechiIdaresi/CreateIstifadechi
        [HttpPost]
        public ActionResult CreateIstifadechi(Istifadechi istifadechi)
        {
            try
            {
                istifadechi.QeydiyyatTarixi = DateTime.Now;
                //istifadechiManager.Add(istifadechi);

                if (ModelState.IsValid)
                {
                    var emeliyyatNeticesi = istifadechiManager.Add(istifadechi);
                    if (emeliyyatNeticesi > 0)
                    {
                        return RedirectToAction("IndexIstifadechi"); //Əməliyyat uğurlu olduqda kitabların siyahısına yönləndirir
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Xəta baş verdi, kitab əlavə olunmadı!");
            }

            return View();
        }

        // GET: Admin/IstifadechiIdaresi/EditIstifadechi/5
        public ActionResult EditIstifadechi(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Istifadechi istifadechi = istifadechiManager.FindById(id.Value);
            if (istifadechi == null)
            {
                return HttpNotFound();
            }

            ViewBag.RolID = new SelectList(rolManager.GetAll(), "RolID", "RolAdi", istifadechi.RolID);

            return View(istifadechi);
        }

        // POST: Admin/IstifadechiIdaresi/EditIstifadechi/5
        [HttpPost]
        public ActionResult EditIstifadechi(Istifadechi istifadechi)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    istifadechiManager.Update(istifadechi);
                }

                return RedirectToAction("IndexIstifadechi");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/IstifadechiIdaresi/DeleteIstifadechi/5
        public ActionResult DeleteIstifadechi(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Istifadechi istifadechi = istifadechiManager.FindById(id.Value);
            if (istifadechi == null)
            {
                return HttpNotFound();
            }

            return View(istifadechi);
        }

        // POST: Admin/IstifadechiIdaresi/DeleteIstifadechi/5
        [HttpPost]
        public ActionResult DeleteIstifadechi(int id, FormCollection collection)
        {
            try
            {
                istifadechiManager.Delete(id);

                return RedirectToAction("IndexIstifadechi");
            }
            catch
            {
                return View();
            }
        }
    }
}
