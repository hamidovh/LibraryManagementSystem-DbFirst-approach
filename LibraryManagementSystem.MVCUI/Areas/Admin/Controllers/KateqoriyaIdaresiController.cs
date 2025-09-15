using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class KateqoriyaIdaresiController : BaseController
    {
        KateqoriyaManager kateqoriyaManager = new KateqoriyaManager();

        // GET: Admin/KateqoriyaIdaresi
        public ActionResult IndexKateqoriya(string searchText, string sortColumn, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var kateqoriya = kateqoriyaManager.GetAll();

            //if (!string.IsNullOrEmpty(searchText))
            //{
            //    string lowerSearch = searchText.ToLower();
            //    kateqoriya = kateqoriya
            //        .Where(k => k.KateqoriyaAdi != null && k.KateqoriyaAdi.ToLower().Contains(lowerSearch))
            //        .ToList();
            //}

            //// Sıralama:
            //if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
            //{
            //    switch (sortColumn)
            //    {
            //        case "KateqoriyaAdi":
            //            if (sortOrder == "asc")
            //                kateqoriya = kateqoriya.OrderBy(k => k.KateqoriyaAdi).ToList();
            //            else if (sortOrder == "desc")
            //                kateqoriya = kateqoriya.OrderByDescending(k => k.KateqoriyaAdi).ToList();
            //            break;
            //    }
            //}

            //ViewBag.SelectedKateqoriyaAdi = sortOrder;

            return View(kateqoriya);
        }

        public ActionResult KateqoriyaPartial(string searchText, string sortColumn, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var kateqoriya = kateqoriyaManager.GetAll();

            if (!string.IsNullOrEmpty(searchText))
            {
                string lowerSearch = searchText.ToLower();
                kateqoriya = kateqoriya
                    .Where(k => k.KateqoriyaAdi != null && k.KateqoriyaAdi.ToLower().Contains(lowerSearch))
                    .ToList();
            }
            // Sıralama:
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
            {
                switch (sortColumn)
                {
                    case "KateqoriyaAdi":
                        if (sortOrder == "asc")
                            kateqoriya = kateqoriya.OrderBy(k => k.KateqoriyaAdi).ToList();
                        else if (sortOrder == "desc")
                            kateqoriya = kateqoriya.OrderByDescending(k => k.KateqoriyaAdi).ToList();
                        break;
                    default:
                        break;
                }
            }
            ViewBag.SelectedKateqoriyaAdi = sortOrder;
            return PartialView("_KateqoriyaPartial", kateqoriya);
        }

        // GET: Admin/KateqoriyaIdaresi/CreateKateqoriya
        public ActionResult CreateKateqoriya()
        {
            return View();
        }

        // POST: Admin/KateqoriyaIdaresi/CreateKateqoriya
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateKateqoriya(Kateqoriya kateqoriya)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var emeliyyatNeticesi = kateqoriyaManager.Add(kateqoriya);
                        if (emeliyyatNeticesi > 0)
                        {
                            TempData["SuccessMessage"] = "Kateqoriya uğurla əlavə olundu!";
                            return RedirectToAction("CreateKateqoriya"); // Eyni səhifəyə yönləndir, mesajı göstər
                        }
                        else ModelState.AddModelError("", "Xəta baş verdi, yenidən cəhd edin!");
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Xəta baş verdi, əməliyyat icra olunmadı!");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Xəta baş verdi! Kateqoriya əlavə edilmədi!");
            }
            
            return View(kateqoriya);
        }

        // GET: Admin/KateqoriyaIdaresi/EditKateqoriya/5
        public ActionResult EditKateqoriya(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kateqoriya kateqoriya = kateqoriyaManager.FindById(id.Value);
            if (kateqoriya == null)
            {
                return HttpNotFound();
            }
            return View(kateqoriya);
        }

        // POST: Admin/KateqoriyaIdaresi/EditKateqoriya/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKateqoriya(Kateqoriya kateqoriya)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Database-də var olan məlumatı al:
                    var original = kateqoriyaManager.FindById(kateqoriya.KateqoriyaID);

                    // Əgər heç bir dəyişiklik edilməyibsə:
                    if (original.KateqoriyaAdi == kateqoriya.KateqoriyaAdi &&
                        original.KateqoriyaTesviri == kateqoriya.KateqoriyaTesviri)
                    {
                        ModelState.AddModelError("", "Heç bir dəyişiklik edilməyib!");
                        return View(kateqoriya);
                    }

                    // Edilibsə dəyişiklikləri tətbiq et:
                    kateqoriyaManager.Update(kateqoriya);

                    TempData["SuccessMessage"] = "Dəyişikliklər uğurla əlavə olundu!";
                    return RedirectToAction("EditKateqoriya", new { id = kateqoriya.KateqoriyaID });
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Xəta baş verdi! Kateqoriya redaktə edilmədi!");
            }

            return View(kateqoriya);
        }

        // GET: Admin/KateqoriyaIdaresi/DeleteKateqoriya/5
        public ActionResult DeleteKateqoriya(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kateqoriya kateqoriya = kateqoriyaManager.FindById(id.Value);
            if (kateqoriya == null)
            {
                return HttpNotFound();
            }
            return View(kateqoriya);
        }

        // POST: Admin/KateqoriyaIdaresi/DeleteKateqoriya/5
        [HttpPost, ActionName("DeleteKateqoriya")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            try
            {
                Kateqoriya kateqoriya = kateqoriyaManager.FindById(id.Value);
                kateqoriyaManager.Delete(kateqoriya.KateqoriyaID);

                TempData["SuccessMessage"] = "Kateqoriya uğurla silindi!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Xəta baş verdi!";
            }

            return RedirectToAction("IndexKateqoriya");
        }
    }
}
