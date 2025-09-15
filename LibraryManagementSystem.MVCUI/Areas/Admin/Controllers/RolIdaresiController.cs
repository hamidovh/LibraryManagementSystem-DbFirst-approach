using System.Linq;
using System.Net;
using System.Web.Mvc;
using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class RolIdaresiController : BaseController
    {
        RolManager rolManager = new RolManager();

        // GET: Admin/RolIdaresi
        public ActionResult IndexRol()
        {
            return View(rolManager.GetAll());
        }

        public ActionResult RolPartial(string searchText, string sortColumn, string sortOrder)
        {
            var rol = rolManager.GetAll();
            if (!string.IsNullOrEmpty(searchText))
            {
                string search = searchText.ToLower();
                rol = rol.Where(r =>
                    r.RolAdi.ToLower().Contains(search)
                ).ToList();
            }
            // Sort:
            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "RolAdi":
                        rol = (sortOrder == "asc")
                            ? rol.OrderBy(r => r.RolAdi).ToList()
                            : rol.OrderByDescending(r => r.RolAdi).ToList();
                        break;
                    default:
                        break;
                }
                // əgər sortColumn boş gəlirsə = heç bir sıralama aparılmır (default olaraq DB qaytarır)
            }
            ViewBag.CurrentSortColumn = sortColumn;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SelectedRolAdi = (sortColumn == "RolAdi") ? sortOrder : "";
            return PartialView("_RolPartial", rol);
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
        public ActionResult CreateRol(Rol rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool eyniVar = rolManager.GetAll().Any(m => m.RolID != rol.RolID && m.RolAdi.ToLower() == rol.RolAdi.ToLower());

                    if (eyniVar)
                    {
                        ModelState.AddModelError("", "Bu rol artıq mövcuddur!");
                        return View(rol);
                    }

                    var emeliyyatNeticesi = rolManager.Add(rol);
                    if (emeliyyatNeticesi > 0)
                    {
                        TempData["SuccessMessage"] = "Rol uğurla əlavə edildi!";
                        return RedirectToAction("CreateRol");
                    }
                    else ModelState.AddModelError("", "Rol əlavə edilərkən xəta baş verdi!");
                }
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("", "Xəta baş verdi! Rol əlavə edilmədi!");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRol(Rol rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Database-də var olan məlumatı al:
                    var original = rolManager.FindById(rol.RolID);

                    // Əgər heç bir dəyişiklik edilməyibsə:
                    if (original.RolAdi == rol.RolAdi)
                    {
                        ModelState.AddModelError("", "Heç bir dəyişiklik edilməyib!");
                        return View(rol);
                    }

                    // Inline yoxlama: ad + soyad kombinasiyasının mövcudluğu
                    bool eyniVar = rolManager.GetAll().Any(m => m.RolID != rol.RolID && m.RolAdi.ToLower() == rol.RolAdi.ToLower());

                    if (eyniVar)
                    {
                        ModelState.AddModelError("", "Bu rol artıq mövcuddur!");
                        return View(rol);
                    }

                    // Əgər yoxdursa, redaktə et:
                    var emeliyyatNeticesi = rolManager.Update(rol);
                    if (emeliyyatNeticesi > 0)
                    {
                        TempData["SuccessMessage"] = "Rol uğurla redaktə olundu!";
                        return RedirectToAction("EditRol", new { id = rol.RolID });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Rol redaktə edilərkən xəta baş verdi!");
                    }
                }
                return View(rol);
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("", "Xəta baş verdi! Rol redaktə edilmədi!");
                return View(rol);
            }
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
            try
            {
                Rol rol = rolManager.FindById(id);
                var emeliyyatNeticesi = rolManager.Delete(rol.RolID);
                if (emeliyyatNeticesi > 0)
                {
                    TempData["SuccessMessage"] = "Rol uğurla silindi!";
                    return RedirectToAction("IndexRol", new { id = id });
                }
                else
                {
                    ModelState.AddModelError("", "Rol silinərkən xəta baş verdi!");
                }
            }
            catch (System.Exception)
            {
                TempData["ErrorMessage"] = "Xəta baş verdi! Rol silinmədi!";
            }

            return RedirectToAction("IndexRol");
        }
    }
}
