using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class MuellifIdaresiController : BaseController
    {
        MuellifManager muellifManager = new MuellifManager();

        // GET: Admin/MuellifIdaresi
        public ActionResult IndexMuellif(string searchText, string sortColumn, string sortOrder, string filterValue)
        {
            var muellif = muellifManager.GetAll();

            //ViewBag.CurrentSortColumn = sortColumn;
            //ViewBag.CurrentSortOrder = sortOrder;
            //ViewBag.SelectedMuellifAdi = (sortColumn == "MuellifAdi") ? sortOrder : "";
            //ViewBag.SelectedMuellifSoyadi = (sortColumn == "MuellifSoyadi") ? sortOrder : "";

            return View(muellif);
        }

        public ActionResult MuellifPartial(string searchText, string sortColumn, string sortOrder)
        {
            var muellif = muellifManager.GetAll();

            if (!string.IsNullOrEmpty(searchText))
            {
                string search = searchText.ToLower();
                muellif = muellif.Where(m =>
                    m.MuellifAdi.ToLower().Contains(search) ||
                    m.MuellifSoyadi.ToLower().Contains(search)
                ).ToList();
            }

            // Sort:
            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "MuellifAdi":
                        muellif = (sortOrder == "asc")
                            ? muellif.OrderBy(m => m.MuellifAdi).ToList()
                            : muellif.OrderByDescending(m => m.MuellifAdi).ToList();
                        break;

                    case "MuellifSoyadi":
                        muellif = (sortOrder == "asc")
                            ? muellif.OrderBy(m => m.MuellifSoyadi).ToList()
                            : muellif.OrderByDescending(m => m.MuellifSoyadi).ToList();
                        break;
                    case "MuellifAdSoyadi":
                        muellif = (sortOrder == "asc")
                            ? muellif.OrderBy(m => m.MuellifAdSoyadi).ToList()
                            : muellif.OrderByDescending(m => m.MuellifAdSoyadi).ToList();
                        break;
                    default:
                        break;
                }
                // əgər sortColumn boş gəlirsə = heç bir sıralama aparılmır (default olaraq DB qaytarır)
            }

            return PartialView("_MuellifPartial", muellif);
        }

        // GET: Admin/MuellifIdaresi/Create
        public ActionResult CreateMuellif()
        {
            return View();
        }

        // POST: Admin/MuellifIdaresi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMuellif(Muellif muellif)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Inline yoxlama: ad + soyad kombinasiyasının mövcudluğu
                    bool eyniVar = muellifManager.GetAll().Any(m => m.MuellifID != muellif.MuellifID && m.MuellifAdi.ToLower() == muellif.MuellifAdi.ToLower() && m.MuellifSoyadi.ToLower() == muellif.MuellifSoyadi.ToLower());

                    if (eyniVar)
                    {
                        ModelState.AddModelError("", "Bu müəllif artıq mövcuddur!");
                        return View(muellif);
                    }

                    // Əgər yoxdursa, əlavə et:
                    var emeliyyatNeticesi = muellifManager.Add(muellif);
                    if (emeliyyatNeticesi > 0)
                    {
                        TempData["SuccessMessage"] = "Müəllif uğurla əlavə olundu!";
                        return RedirectToAction("CreateMuellif");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Müəllif əlavə edilərkən xəta baş verdi!");
                    }
                }
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("", "Xəta baş verdi! Müəllif əlavə edilmədi!");
            }

            // ModelState valid deyil və ya əlavə uğursuz olduqda view-ə qayıt:
            return View(muellif);
        }

        // GET: Admin/MuellifIdaresi/Edit/5
        public ActionResult EditMuellif(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Muellif muellif = muellifManager.FindById(id.Value);
            if (muellif == null)
            {
                return HttpNotFound();
            }
            return View(muellif);
        }

        // POST: Admin/MuellifIdaresi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMuellif(Muellif muellif)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Database-də var olan məlumatı al:
                    var original = muellifManager.FindById(muellif.MuellifID);

                    // Əgər heç bir dəyişiklik edilməyibsə:
                    if (original.MuellifAdi == muellif.MuellifAdi &&
                        original.MuellifSoyadi == muellif.MuellifSoyadi)
                    {
                        ModelState.AddModelError("", "Heç bir dəyişiklik edilməyib!");
                        return View(muellif);
                    }

                    // Inline yoxlama: ad + soyad kombinasiyasının mövcudluğu
                    bool eyniVar = muellifManager.GetAll().Any(m => m.MuellifID != muellif.MuellifID && m.MuellifAdi.ToLower() == muellif.MuellifAdi.ToLower() && m.MuellifSoyadi.ToLower() == muellif.MuellifSoyadi.ToLower());

                    if (eyniVar)
                    {
                        ModelState.AddModelError("", "Bu müəllif artıq mövcuddur!");
                        return View(muellif);
                    }

                    // Əgər yoxdursa, redaktə et:
                    var emeliyyatNeticesi = muellifManager.Update(muellif);
                    if (emeliyyatNeticesi > 0)
                    {
                        TempData["SuccessMessage"] = "Müəllif uğurla redaktə olundu!";
                        return RedirectToAction("EditMuellif", new { id = muellif.MuellifID });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Müəllif redaktə edilərkən xəta baş verdi!");
                    }
                }
                return View(muellif);
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("", "Xəta baş verdi! Müəllif redaktə edilmədi!");
                return View(muellif);
            }
        }

        // GET: Admin/MuellifIdaresi/Delete/5
        public ActionResult DeleteMuellif(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Muellif muellif = muellifManager.FindById(id.Value);
            if (muellif == null)
            {
                return HttpNotFound();
            }
            return View(muellif);
        }

        // POST: Admin/MuellifIdaresi/Delete/5
        [HttpPost, ActionName("DeleteMuellif")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Muellif muellif = muellifManager.FindById(id);
                var emeliyyatNeticesi = muellifManager.Delete(muellif.MuellifID);
                if (emeliyyatNeticesi > 0)
                {
                    TempData["SuccessMessage"] = "Müəllif uğurla silindi!";
                    return RedirectToAction("IndexMuellif");
                }
                else
                {
                    //ModelState.AddModelError("", "Müəllif silinərkən xəta baş verdi!");
                    TempData["ErrorMessage"] = "Müəllif silinərkən xəta baş verdi!";
                    return RedirectToAction("IndexMuellif");
                }
            }
            catch (System.Exception)
            {
                //ModelState.AddModelError("", "Xəta baş verdi! Müəllif silimədi!");
                TempData["ErrorMessage"] = "Xəta baş verdi! Müəllif silimədi!";
            }

            return RedirectToAction("IndexMuellif", new { id = id });
        }
    }
}

/*
Problem: ModelState.AddModelError işləməyəcək.
ModelState.AddModelError istifadə olunur, amma sonunda RedirectToAction edilir. ModelState məlumatları redirect zamanı itir, yəni istifadəçi heç bir xəta mesajı görməyəcək. Bunun əvəzinə, xətaları TempData və ya ViewBag vasitəsilə ötürmək lazımdır ki, redirect sonrası da mesajlar göstərilsin.
*/
//Delete üçün View-da ActionLink əvəzinə form + POST istifadə etmək daha təhlükəsizdir (CSRF üçün).
