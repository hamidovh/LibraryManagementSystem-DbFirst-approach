using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class CerimeIdaresiController : BaseController
    {
        CerimeManager cerimeManager = new CerimeManager();
        IcareManager icareManager = new IcareManager();
        IstifadechiManager istifadechiManager = new IstifadechiManager();

        // GET: Admin/CerimeIdaresi
        public ActionResult IndexCerime(string searchText, string sortColumn, string sortOrder, string filterValue)
        {
            var cerime = cerimeManager.GetAllByInclude(c => c.Icare, c => c.Istifadechi);
            var cerim = cerimeManager.GetAll();

            var istifadechiler = istifadechiManager.GetAll().Select(r => r.IstifadechiAdi).Distinct().ToList();
            ViewBag.Istifadechiler = istifadechiler;

            var icareler = icareManager.GetAll().Select(r => r.IcareID).Distinct().ToList();
            ViewBag.Icareler = icareler;

            if (!string.IsNullOrEmpty(searchText))
            {
                string lowerSearch = searchText.ToLower();
                cerim = cerim
                    .Where(c =>
                        (c.Istifadechi != null && c.Istifadechi.Adi != null && c.Istifadechi.Adi.ToLower().Contains(lowerSearch)) ||
                        (c.Istifadechi != null && c.Istifadechi.Soyadi != null && c.Istifadechi.Soyadi.ToLower().Contains(lowerSearch))
                    ).ToList();
                return View(cerim);
            }

            // Sort/Filter:
            switch (sortColumn)
            {
                case "Istifadechi":
                    if (sortOrder == "asc")
                        cerim = cerim.OrderBy(c => c.Istifadechi.Adi).ThenBy(c => c.Istifadechi.Soyadi).ToList();
                    else if (sortOrder == "desc")
                        cerim = cerim.OrderByDescending(c => c.Istifadechi.Adi).ThenByDescending(c => c.Istifadechi.Soyadi).ToList();
                    else
                        // Default: indexdəki sıralama
                        cerim = cerim.OrderBy(c => c.IcareID).ToList();
                    break;

                case "IcareID":
                    if (sortOrder == "asc")
                        cerim = cerim.OrderBy(c => c.IcareID).ToList();
                    else if (sortOrder == "desc")
                        cerim = cerim.OrderByDescending(c => c.IcareID).ToList();
                    else
                        cerim = cerim.OrderBy(c => c.IcareID).ToList();
                    break;

                case "Sebeb":
                    if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                        cerim = cerim.Where(c => c.Sebeb == filterValue).ToList();
                    else
                        cerim = cerim.OrderBy(c => c.IcareID).ToList();
                    break;

                case "CerimeTarixi":
                    if (sortOrder == "asc")
                        cerim = cerim.OrderBy(c => c.CerimeTarixi).ToList();
                    else if (sortOrder == "desc")
                        cerim = cerim.OrderByDescending(c => c.CerimeTarixi).ToList();
                    else
                        cerim = cerim.OrderBy(c => c.IcareID).ToList();
                    break;

                case "Mebleg":
                    if (filterValue == "Azdan-çoxa")
                        cerim = cerim.OrderBy(c => c.Mebleg).ToList();
                    else if (filterValue == "Çoxdan-aza")
                        cerim = cerim.OrderByDescending(c => c.Mebleg).ToList();
                    else if (filterValue == "Hamısı" || string.IsNullOrEmpty(filterValue))
                        // default: IcareID ilə
                        cerim = cerim.OrderBy(c => c.IcareID).ToList();
                    break;

                case "Odenilibmi":
                    if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                    {
                        if (filterValue == "Ödənilmiş")
                            cerim = cerim.Where(c => c.Odenilibmi == true).ToList();
                        else if (filterValue == "Ödənilməmiş")
                            cerim = cerim.Where(c => c.Odenilibmi == false).ToList();
                    }
                    else
                        // Hamısı və ya boş seçildikdə:
                        cerim = cerim.OrderBy(c => c.IcareID).ToList();
                    break;

                case "OdenmeTarixi":
                    if (sortOrder == "asc")
                        cerim = cerim.OrderBy(c => c.OdenmeTarixi ?? DateTime.MaxValue).ToList();
                    else if (sortOrder == "desc")
                        cerim = cerim.OrderByDescending(c => c.OdenmeTarixi ?? DateTime.MinValue).ToList();
                    else
                        // Hamısı və ya boş seçildikdə:
                        cerim = cerim.OrderBy(c => c.IcareID).ToList();
                    break;

                default:
                    // Default sort: CerimeID artan sıra ilə
                    cerim = cerim.OrderBy(c => c.CerimeID).ToList();
                    break;
            }

            // ViewBag-lər sort və filter seçimi üçün:
            ViewBag.CurrentSortColumn = sortColumn;
            ViewBag.CurrentSortOrder = sortOrder;

            ViewBag.SelectedIstifadechi = (sortColumn == "Istifadechi") ? sortOrder : "";
            ViewBag.SelectedIcareID = (sortColumn == "IcareID") ? sortOrder : "";
            ViewBag.SelectedSebeb = (sortColumn == "Sebeb") ? filterValue : "";
            ViewBag.SelectedCerimeTarixi = (sortColumn == "CerimeTarixi") ? sortOrder : "";
            ViewBag.SelectedMebleg = (sortColumn == "Mebleg") ? filterValue : "";
            ViewBag.SelectedOdenilibmi = (sortColumn == "Odenilibmi") ? filterValue : "";
            ViewBag.SelectedOdenmeTarixi = (sortColumn == "OdenmeTarixi") ? sortOrder : "";

            return View(cerim);
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
            // Bütün icarələri götür:
            // Sadəcə Statusu "Gecikir" olanlar DropDown-da seçiləbilən olsun:
            var allIcare = icareManager.GetAll().Select(i => new SelectListItem
            {
                Value = i.IcareID.ToString(),
                Text = i.IcareID + " - " + i.Statusu,
                Disabled = i.Statusu != "Gecikir" // əgər "Gecikir" deyilsə disabled
            }).ToList();

            // ViewBag-ə göndəririk:
            ViewBag.IcareList = allIcare;

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
            try
            {
                // Əgər eyni IcareID-li cərimə artıq mövcuddursa:
                bool movcudCerime = cerimeManager.GetAll().Any(c => c.IcareID == cerime.IcareID);
                if (movcudCerime)
                {
                    ModelState.AddModelError("IcareID", "Bu cərimə artıq mövcuddur!");
                }

                if (ModelState.IsValid)
                {
                    cerimeManager.Add(cerime);
                    TempData["SuccessMessage"] = "Cərimə uğurla əlavə olundu!";
                    return RedirectToAction("CreateCerime"); // yenidən səhifəyə yönləndir ki, modal açılsın
                }
            }
            catch (Exception ex)
            {
                // Hər hansı digər xəta baş verdikdə ModelState-ə əlavə edirik:
                ModelState.AddModelError("", "Xəta baş verdi, cərimə əlavə olunmadı! " + ex.Message);
            }

            // Əgər səhv varsa, DropDown-ları yenidən doldururuq:
            var allIcare = icareManager.GetAll().Select(i => new SelectListItem
            {
                Value = i.IcareID.ToString(),
                Text = i.IcareID + " - " + i.Statusu,
                Disabled = i.Statusu != "Gecikir"
            }).ToList();

            ViewBag.IcareList = allIcare;

            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", cerime.IstifadechiID);
            ViewBag.StatusList = new SelectList(new List<string> { "Gecikir", "İtirilib" }, cerime.Sebeb);

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

            // DropDown-ları göndəririk:
            ViewBag.IcareID = new SelectList(icareManager.GetAll(), "IcareID", "IcareID", cerime.IcareID);
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", cerime.IstifadechiID);

            // Burada mövcud dəyəri seçirik:
            ViewBag.StatusList = new SelectList(new List<string> { "Gecikir", "İtirilib" }, cerime.Sebeb);

            // Tarixi avtomatik modeldən gələcək

            return View(cerime);
        }
        
        // POST: Admin/CerimeIdaresi/EditCerime/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCerime(Cerime cerime)
        {
            try
            {
                // Əgər eyni IcareID-li başqa bir cərimə artıq mövcuddursa:
                bool eyniCerime = cerimeManager.GetAll()
                    .Any(c => c.IcareID == cerime.IcareID && c.CerimeID != cerime.CerimeID);

                if (eyniCerime)
                {
                    ModelState.AddModelError("IcareID", "Bu cərimə artıq mövcuddur!");
                }

                // Ödəniş yoxlamaları:
                if (cerime.Odenilibmi && !cerime.OdenmeTarixi.HasValue)
                {
                    ModelState.AddModelError("OdenmeTarixi", "Cərimə ödənilibsə, ödəniş tarixini də qeyd edin!");
                }

                if (cerime.OdenmeTarixi.HasValue && !cerime.Odenilibmi)
                {
                    ModelState.AddModelError("Odenilibmi", "Cərimə ödənilibsə, ödənilməni təsdiqləyin!");
                }

                if (ModelState.IsValid)
                {
                    // Əgər dəyişiklik yoxdursa:
                    var movcudCerime = cerimeManager.FindById(cerime.CerimeID);
                    if (movcudCerime.IcareID == cerime.IcareID &&
                        movcudCerime.IstifadechiID == cerime.IstifadechiID &&
                        movcudCerime.Sebeb == cerime.Sebeb &&
                        movcudCerime.CerimeTarixi == cerime.CerimeTarixi &&
                        movcudCerime.Mebleg == cerime.Mebleg &&
                        movcudCerime.Odenilibmi == cerime.Odenilibmi &&
                        movcudCerime.OdenmeTarixi == cerime.OdenmeTarixi)
                    {
                        ModelState.AddModelError("", "Heç bir dəyişiklik edilməyib!");
                    }
                    else
                    {
                        // Update əməliyyatı icra olunur:
                        var emeliyyatNeticesi = cerimeManager.Update(cerime);
                        if (emeliyyatNeticesi > 0)
                        {
                            TempData["SuccessMessage"] = "Cərimə uğurla redaktə olundu!";
                            return RedirectToAction("EditCerime", new { id = cerime.CerimeID });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Redaktə zamanı xəta baş verdi!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Xəta baş verdi, cərimə redaktə olunmadı! " + ex.Message);
            }

            // Əgər səhv varsa, DropDown-ları yenidən doldururuq:
            ViewBag.IcareID = new SelectList(icareManager.GetAll(), "IcareID", "IcareID", cerime.IcareID);
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", cerime.IstifadechiID);
            ViewBag.StatusList = new SelectList(new List<string> { "Gecikir", "İtirilib" }, cerime.Sebeb);

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
            try
            {
                Cerime cerime = cerimeManager.FindById(id);
                if (cerime == null)
                {
                    TempData["ErrorMessage"] = "Cərimə tapılmadı!";
                    return RedirectToAction("IndexCerime");
                }

                // Şərt: İcarə qaytarılıb və cərimə ödənilibsə
                bool icareQaytarilib = cerime.Icare != null && cerime.Icare.Statusu == "Qaytarılıb";
                bool cerimeOdenilib = cerime.Odenilibmi && cerime.OdenmeTarixi.HasValue;

                if (!icareQaytarilib && !cerimeOdenilib)
                {
                    TempData["ErrorMessage"] = "Bu cəriməni silmək mümkün deyil! Cərimə ödənilməli və icarə qaytarılmalıdır.";
                    return RedirectToAction("IndexCerime");
                }

                // Silmə əməliyyatı
                var emeliyyatNeticesi = cerimeManager.Delete(id);
                if (emeliyyatNeticesi > 0)
                {
                    TempData["SuccessMessage"] = "Cərimə uğurla silindi!";
                    return RedirectToAction("IndexCerime");
                }
                else
                    TempData["ErrorMessage"] = "Cərimə silinmədi!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Cəriməni silərkən xəta baş verdi: " + ex.Message;
            }

            return RedirectToAction("IndexCerime");
        }
    }
}
