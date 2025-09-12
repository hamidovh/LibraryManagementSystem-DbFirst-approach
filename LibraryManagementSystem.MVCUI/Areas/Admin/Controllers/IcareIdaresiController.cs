using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebGrease;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class IcareIdaresiController : Controller
    {
        IcareManager icareManager = new IcareManager();
        IstifadechiManager istifadechiManager = new IstifadechiManager();
        KitabManager kitabManager = new KitabManager();

        // GET: Admin/IcareIdaresi
        public ActionResult IndexIcare(string searchText, string sortColumn, string sortOrder, string filterValue, string statusFilter, string qaytarilibFilter, string qiymetFilter)
        {
            var icare = icareManager.GetAllByInclude(i => i.Istifadechi, i => i.Kitab);
            var icar = icareManager.GetAll();

            var istifadechiler = istifadechiManager.GetAll().Select(r => r.IstifadechiAdi).Distinct().ToList();
            ViewBag.Istifadechiler = istifadechiler;

            var kitablarr = kitabManager.GetAll().Select(r => r.KitabAdi).Distinct().ToList();
            ViewBag.Kitablar = kitablarr;

            // Axtar:
            if (!string.IsNullOrEmpty(searchText))
            {
                string lowerSearch = searchText.ToLower();
                icar = icar
                    .Where(i =>
                        (i.Istifadechi != null && i.Istifadechi.Adi != null && i.Istifadechi.Adi.ToLower().Contains(lowerSearch)) ||
                        (i.Istifadechi != null && i.Istifadechi.Soyadi != null && i.Istifadechi.Soyadi.ToLower().Contains(lowerSearch)) ||
                        (i.Kitab != null && i.Kitab.KitabAdi != null && i.Kitab.KitabAdi.ToLower().Contains(lowerSearch))
                    ).ToList();
                return View(icar);
            }

            // Sort/Filter:
            switch (sortColumn)
            {
                case "Istifadechi":
                    icar = sortOrder == "desc"
                        ? icar.OrderByDescending(i => i.Istifadechi.AdSoyadi).ToList()
                        : icar.OrderBy(i => i.Istifadechi.AdSoyadi).ToList();
                    break;

                case "Kitab":
                    icar = sortOrder == "desc"
                        ? icar.OrderByDescending(i => i.Kitab.KitabAdi).ToList()
                        : icar.OrderBy(i => i.Kitab.KitabAdi).ToList();
                    break;

                case "IcareTarixi":
                    icar = sortOrder == "desc"
                        ? icar.OrderByDescending(i => i.IcareTarixi).ToList()
                        : icar.OrderBy(i => i.IcareTarixi).ToList();
                    break;

                case "SonTarix":
                    icar = sortOrder == "desc"
                        ? icar.OrderByDescending(i => i.SonTarix).ToList()
                        : icar.OrderBy(i => i.SonTarix).ToList();
                    break;
                case "Statusu":
                    if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                        icar = icar.Where(i => i.Statusu == filterValue).ToList();
                    break;
                case "Qaytarilibmi":
                    if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                    {
                        if (filterValue == "Qaytarılmış")
                            icar = icar.Where(i => i.Qaytarilibmi == true).ToList();
                        else if (filterValue == "Qaytarılmamış")
                            icar = icar.Where(i => i.Qaytarilibmi == false).ToList();
                    }
                    break;
                case "IcareQiymeti":
                    if (string.IsNullOrEmpty(filterValue) || filterValue == "Hamısı")
                    {
                        // heç nə etmə, default siyahı göstərilsin
                    }
                    else if (filterValue == "Ucuzdan-bahaya")
                    {
                        icar = icar.OrderBy(i => i.IcareQiymeti).ToList();
                    }
                    else if (filterValue == "Bahadan-ucuza")
                    {
                        icar = icar.OrderByDescending(i => i.IcareQiymeti).ToList();
                    }
                    break;
                default:
                    //icare = icare.OrderBy(i => i.IcareID).ToList();
                    break;
            }

            // ViewBag-lər sort və filter seçimi üçün:
            ViewBag.CurrentSortColumn = sortColumn;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SelectedIstifadechi = (sortColumn == "Istifadechi") ? sortOrder : "";
            ViewBag.SelectedKitab = (sortColumn == "Kitab") ? sortOrder : "";
            ViewBag.SelectedIcareTarixi = (sortColumn == "IcareTarixi") ? sortOrder : "";
            ViewBag.SelectedSonTarix = (sortColumn == "SonTarix") ? sortOrder : "";
            ViewBag.SelectedStatusu = (sortColumn == "Statusu") ? filterValue : "";
            ViewBag.SelectedQaytarilibmi = (sortColumn == "Qaytarilibmi") ? filterValue : "";
            ViewBag.SelectedIcareQiymeti = (sortColumn == "IcareQiymeti") ? filterValue : "";
            //ViewBag.StatusuList = new SelectList(new List<string> { "Hamısı", "Aktiv", "Gecikir", "Qaytarılıb" });

            return View(icar);
        }

        // GET: Admin/IcareIdaresi/CreateIcare
        public ActionResult CreateIcare()
        {
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi");
            ViewBag.KitabID = new SelectList(kitabManager.GetAll(), "KitabID", "KitabAdi");

            ViewBag.StatusList = new SelectList(new List<string> { "Aktiv", "Gecikir", "Qaytarılıb" });

            return View(new Icare());
        }

        // POST: Admin/IcareIdaresi/CreateIcare
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIcare(Icare icare)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Mövcud icarələri yoxla (eyni istifadəçi və kitab üzrə):
                    var movcudIcare = icareManager.GetAll()
                        .FirstOrDefault(i => i.IstifadechiID == icare.IstifadechiID
                                          && i.KitabID == icare.KitabID
                                          && i.Qaytarilibmi == false); // yalnız qaytarılmamış icarələr

                    if (movcudIcare != null)
                    {
                        ModelState.AddModelError("", "Bu icarə artıq mövcuddur!");
                    }
                    else
                    {
                        var emeliyyatNeticesi = icareManager.Add(icare);

                        if (emeliyyatNeticesi > 0)
                        {
                            TempData["SuccessMessage"] = "İcarə uğurla əlavə olundu!";
                            return RedirectToAction("IndexIcare");
                        }
                        else
                        {
                            ModelState.AddModelError("", "İcarə əlavə olunarkən xəta baş verdi!");
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                var msg = ex.InnerException?.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError("", "Xəta: " + msg);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Xəta baş verdi, icarə əlavə olunmadı! " + ex.Message);
            }

            // Əgər xəta olarsa, yenidən lazımi ViewBag-ləri doldur:
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", icare.IstifadechiID);
            ViewBag.KitabID = new SelectList(kitabManager.GetAll(), "KitabID", "KitabAdi", icare.KitabID);
            ViewBag.StatusList = new SelectList(new List<string> { "Aktiv", "Gecikir", "Qaytarılıb" }, icare.Statusu);

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
            ViewBag.StatusList = new SelectList(new List<string> { "Aktiv", "Gecikir", "Qaytarılıb" }, icare.Statusu);
            return View(icare);
        }

        // POST: Admin/IcareIdaresi/EditIcare/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIcare(Icare icare)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbIcare = icareManager.FindById(icare.IcareID);
                    if (dbIcare == null)
                    {
                        return HttpNotFound();
                    }

                    // Əlavə şərt: yalnız "Qaytarılıb" statusunda qaytarılma mümkün olsun
                    if ((icare.Statusu == "Aktiv" || icare.Statusu == "Gecikir")
                        && (icare.Qaytarilibmi || icare.QaytarilmaTarixi.HasValue))
                    {
                        ModelState.AddModelError("", "Aktiv və ya gecikmiş icarəni qaytarılmış kimi qeyd etmək mümkün deyil!");
                    }
                    else
                    {
                        // Eyni istifadəçi və kitab üzrə aktiv icarə yoxla (ancaq başqa ID-lə):
                        var movcudIcare = icareManager.GetAll().FirstOrDefault(i => i.IstifadechiID == icare.IstifadechiID && i.KitabID == icare.KitabID && i.Qaytarilibmi == false && i.IcareID != icare.IcareID);

                        if (movcudIcare != null)
                        {
                            ModelState.AddModelError("", "Bu icarə artıq mövcuddur!");
                        }
                        else
                        {
                            // Əvvəlki ilə müqayisə et, dəyişiklik olub-olmadığını yoxla:
                            bool deyisiklikVar = dbIcare.IstifadechiID != icare.IstifadechiID
                                              || dbIcare.KitabID != icare.KitabID
                                              || dbIcare.IcareTarixi != icare.IcareTarixi
                                              || dbIcare.SonTarix != icare.SonTarix
                                              || dbIcare.QaytarilmaTarixi != icare.QaytarilmaTarixi
                                              || dbIcare.Statusu != icare.Statusu
                                              || dbIcare.Qaytarilibmi != icare.Qaytarilibmi
                                              || dbIcare.IcareQiymeti != icare.IcareQiymeti;

                            if (!deyisiklikVar)
                            {
                                ModelState.AddModelError("", "Heç bir dəyişiklik edilməyib!");
                            }
                            else
                            {
                                // Update icra et:
                                var emeliyyatNeticesi = icareManager.Update(icare);
                                if (emeliyyatNeticesi > 0)
                                {
                                    TempData["SuccessMessage"] = "İcarə uğurla redaktə olundu!";
                                    return RedirectToAction("EditIcare", new { id = icare.IcareID });
                                }
                                else
                                {
                                    ModelState.AddModelError("", "İcarə redaktə olunarkən xəta baş verdi!");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Redaktə zamanı xəta baş verdi! " + ex.Message);
            }

            // Əgər səhv varsa ViewBag-ləri doldur və səhifəyə geri qayıt:
            ViewBag.IstifadechiID = new SelectList(istifadechiManager.GetAll(), "IstifadechiID", "AdSoyadi", icare.IstifadechiID);
            ViewBag.KitabID = new SelectList(kitabManager.GetAll(), "KitabID", "KitabAdi", icare.KitabID);
            ViewBag.StatusList = new SelectList(new List<string> { "Aktiv", "Gecikir", "Qaytarılıb" }, icare.Statusu);

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
            try
            {
                Icare icare = icareManager.FindById(id);
                if (icare == null)
                {
                    TempData["ErrorMessage"] = "İcarə tapılmadı, silinmə baş vermədi!";
                    return RedirectToAction("IndexIcare");
                }

                // Sadəcə qaytarılmanı yoxlayırıq:
                if (icare.Statusu == "Qaytarılıb" && icare.QaytarilmaTarixi.HasValue && icare.Qaytarilibmi)
                {
                    var emeliyyatNeticesi = icareManager.Delete(id);
                    TempData["SuccessMessage"] = emeliyyatNeticesi > 0
                        ? "İcarə uğurla silindi!"
                        : "İcarə silinmədi!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Bu icarəni silmək mümkün deyil! Əvvəlcə icarə qaytarılmalı və varsa bütün cərimələri ödənilməlidir.";
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("An error occurred while updating the entries"))
                {
                    TempData["ErrorMessage"] = "İcarə hələki qaytarılmayıb. Qaytarılmamış icarəni silmək olmaz!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Xəta baş verdi: " + ex.Message;
                }
            }

            return RedirectToAction("IndexIcare");
        }
    }
}
