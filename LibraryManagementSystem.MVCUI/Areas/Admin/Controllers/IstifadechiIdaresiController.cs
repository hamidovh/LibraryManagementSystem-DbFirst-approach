using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class IstifadechiIdaresiController : Controller
    {
        IstifadechiManager istifadechiManager = new IstifadechiManager();
        RolManager rolManager = new RolManager();

        // GET: Admin/IstifadechiIdaresi
        public ActionResult IndexIstifadechi(string searchText)
        {
            var istifadechi = istifadechiManager.GetAll();

            if (!string.IsNullOrEmpty(searchText))
            {
                istifadechi = istifadechi
                    .Where(i =>
                        (i.Adi != null && i.Adi.Contains(searchText)) ||
                        (i.Soyadi != null && i.Soyadi.Contains(searchText)) ||
                        (i.Email != null && i.Email.Contains(searchText)) ||
                        (i.IstifadechiAdi != null && i.IstifadechiAdi.Contains(searchText)) ||
                        (i.Rol != null && i.Rol.RolAdi.Contains(searchText))
                    ).ToList();
            }

            return View(istifadechi);
        }

        // GET: Admin/IstifadechiIdaresi/CreateIstifadechi
        public ActionResult CreateIstifadechi()
        {
            ViewBag.RolID = new SelectList(rolManager.GetAll(), "RolID", "RolAdi");

            return View();
        }

        // POST: Admin/IstifadechiIdaresi/CreateIstifadechi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIstifadechi(Istifadechi istifadechi)
        {
            try
            {
                // Təkrar istifadəçi adı yoxlanışı
                if (istifadechiManager.GetAll().Any(i => i.IstifadechiAdi == istifadechi.IstifadechiAdi))
                {
                    ModelState.AddModelError("IstifadechiAdi", "Bu istifadəçi adı artıq mövcuddur");
                }

                if (ModelState.IsValid)
                {
                    istifadechi.QeydiyyatTarixi = DateTime.Now;
                    var emeliyyatNeticesi = istifadechiManager.Add(istifadechi);

                    if (emeliyyatNeticesi > 0)
                    {
                        return RedirectToAction("IndexIstifadechi"); // uğurlu olduqda siyahıya qayıdır
                    }
                    else
                    {
                        ModelState.AddModelError("", "İstifadəçi əlavə olunarkən xəta baş verdi!");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Xəta baş verdi, istifadəçi əlavə olunmadı!");
            }

            // Əgər səhv varsa, yenidən rolları doldurmaq lazımdır
            ViewBag.RolID = new SelectList(rolManager.GetAll(), "RolID", "RolAdi", istifadechi.RolID);

            return View(istifadechi);
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
