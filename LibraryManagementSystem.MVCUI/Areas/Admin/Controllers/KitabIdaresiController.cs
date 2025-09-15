using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.MVCUI.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class KitabIdaresiController : BaseController
    {
        KitabManager kitabManager = new KitabManager();
        MuellifManager muellifManager = new MuellifManager();
        KateqoriyaManager kateqoriyaManager = new KateqoriyaManager();

        // GET: Admin/KitabIdaresi/IndexKitab
        public ActionResult IndexKitab(string searchText, int? muellifId, int? kateqoriyaId, string sortColumn, string sortOrder, string filterValue, string icareSort)
        {
            var kitab = kitabManager.GetAll();

            if (!string.IsNullOrEmpty(searchText))
            {
                string search = searchText.ToLower();

                kitab = kitab.Where(k =>
                    k.KitabAdi.ToLower().Contains(search) ||
                    (k.Muellif != null && k.Muellif.Any(m => m.MuellifAdSoyadi.ToLower().Contains(search))) ||
                    (k.Kateqoriya != null && k.Kateqoriya.Any(ka => ka.KateqoriyaAdi.ToLower().Contains(search)))
                ).ToList();
            }

            if (muellifId.HasValue)
            {
                kitab = kitab.Where(k => k.Muellif.Any(m => m.MuellifID == muellifId.Value)).ToList();
            }

            if (kateqoriyaId.HasValue)
            {
                kitab = kitab.Where(k => k.Kateqoriya.Any(ka => ka.KateqoriyaID == kateqoriyaId.Value)).ToList();
            }

            // Sıralama və Filter:
            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "KitabAdi":
                        kitab = (sortOrder == "asc")
                            ? kitab.OrderBy(k => k.KitabAdi).ToList()
                            : (sortOrder == "desc")
                                ? kitab.OrderByDescending(k => k.KitabAdi).ToList()
                                : kitab.OrderBy(k => k.KitabID).ToList();
                        break;
                    case "MuellifID":
                        // Əgər kitabın müəllifi varsa, ilkini götürüb sıralayırıq:
                        kitab = (sortOrder == "asc")
                            ? kitab.OrderBy(k => k.Muellif.Select(m => m.MuellifAdSoyadi).FirstOrDefault()).ToList()
                            : kitab.OrderByDescending(k => k.Muellif.Select(m => m.MuellifAdSoyadi).FirstOrDefault()).ToList();
                        break;

                    case "KateqoriyaID":
                        // Əgər kitabın kateqoriyası varsa, ilkini götürüb sıralayırıq:
                        kitab = (sortOrder == "asc")
                            ? kitab.OrderBy(k => k.Kateqoriya.Select(ka => ka.KateqoriyaAdi).FirstOrDefault()).ToList()
                            : kitab.OrderByDescending(k => k.Kateqoriya.Select(ka => ka.KateqoriyaAdi).FirstOrDefault()).ToList();
                        break;
                    case "StokdaVarmi":
                        if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                        {
                            if (filterValue == "Var olanlar")
                                kitab = kitab.Where(k => k.StokdaVarmi == true).ToList();
                            else if (filterValue == "Olmayanlar")
                                kitab = kitab.Where(k => k.StokdaVarmi == false).ToList();
                        }
                        break;
                    default:
                        break;
                }
            }

            // IcareQiymeti filter/order:
            if (!string.IsNullOrEmpty(icareSort))
            {
                switch (icareSort)
                {
                    case "UcuzdanBahaya":
                        kitab = kitab.OrderBy(k => k.IcareQiymeti ?? 0).ToList();
                        break;
                    case "BahadanUcuz":
                        kitab = kitab.OrderByDescending(k => k.IcareQiymeti ?? 0).ToList();
                        break;
                    case "Odenissiz":
                        kitab = kitab.Where(k => !k.IcareQiymeti.HasValue || k.IcareQiymeti.Value == 0).ToList();
                        break;
                    case "Hamisi":
                    default:
                        break;
                }
            }

            ViewBag.CurrentSortColumn = sortColumn;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SelectedKitabAdi = (sortColumn == "KitabAdi") ? sortOrder : "";

            // Dropdowns:
            ViewBag.MuellifID = new SelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", muellifId);
            ViewBag.KateqoriyaID = new SelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kateqoriyaId);

            // StokdaVarmi dropdown üçün ViewBag:
            ViewBag.SelectedStokdaVarmi = (sortColumn == "StokdaVarmi") ? filterValue : "";

            // IcareQiymeti dropdown üçün ViewBag:
            ViewBag.IcareQiymetiSort = icareSort;

            return View(kitab);
        }

        // GET: Admin/KitabIdaresi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kitab kitab = kitabManager.GetAllByIncludes(k => k.Muellif, k => k.Kateqoriya)
                .FirstOrDefault(k => k.KitabID == id.Value);
            if (kitab == null)
            { 
                return HttpNotFound();
            }

            return View(kitab);
        }

        // GET: Admin/KitabIdaresi/CreateKitab
        public ActionResult CreateKitab()
        {
            var kitabViewModel = new KitabViewModel
            {
                MuellifList = new MultiSelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi"),
                KateqoriyaList = new MultiSelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi")
            };

            return View(kitabViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateKitab(KitabViewModel kitabViewModel, HttpPostedFileBase Foto)
        {
            try
            {
                if (ModelState.IsValid) //Boş qalmamalı olan EditorForların dolu gəlib-gəlmədiyini yoxlayır
                {
                    // Əvvəlcə eyni adda kitab varmı? yoxla:
                    // mövcud kitabları gətir:
                    var movcudKitablar = kitabManager.GetAllByInclude(k => k.Muellif)
                        .Where(k => k.KitabAdi.ToLower() == kitabViewModel.KitabAdi.ToLower())
                        .ToList();

                    // indi yoxla: seçilmiş müəlliflərdən hər hansı biri mövcuddursa, artıq kitab var deməkdir
                    bool eyniVar = movcudKitablar.Any(k =>
                        k.Muellif.Any(m => kitabViewModel.sechilmishMuellifIDler.Contains(m.MuellifID)));

                    if (eyniVar)
                    {
                        ModelState.AddModelError("", "Bu kitab həmin müəllif(lər) ilə artıq mövcuddur!");
                    }
                    else
                    {
                        // Yeni kitab obyektini yaradırıq:
                        var kitab = new Kitab
                        {
                            KitabAdi = kitabViewModel.KitabAdi,
                            Haqqinda = kitabViewModel.Haqqinda,
                            IcareQiymeti = kitabViewModel.IcareQiymeti,
                            StokdaVarmi = kitabViewModel.StokdaVarmi,
                            Muellif = new List<Muellif>(),
                            Kateqoriya = new List<Kateqoriya>()
                        };

                        // Müəllifləri əlavə et:
                        foreach (var muellifId in kitabViewModel.sechilmishMuellifIDler)
                        {
                            var muellif = new Muellif { MuellifID = muellifId };
                            muellifManager.Attach(muellif); // Repositorydə Attach metodu var
                            kitab.Muellif.Add(muellif);
                        }

                        // Kateqoriyaları əlavə et:
                        foreach (var kateqoriyaId in kitabViewModel.sechilmishKateqoriyaIDler)
                        {
                            var kateqoriya = new Kateqoriya { KateqoriyaID = kateqoriyaId };
                            kateqoriyaManager.Attach(kateqoriya);
                            kitab.Kateqoriya.Add(kateqoriya);
                        }

                        //Foto əlavə et:
                        if (Foto != null && Foto.ContentLength > 0)
                        {
                            // Serverdə Images qovluğu yolunu müəyyən edirik:
                            string directory = Server.MapPath("~/Images/");

                            // Qovluq yoxlanır, yoxdursa yaradılır:
                            if (!System.IO.Directory.Exists(directory))
                                System.IO.Directory.CreateDirectory(directory);

                            // Orijinal fayl adını və extension-ı alırıq:
                            var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(Foto.FileName);
                            var extension = System.IO.Path.GetExtension(Foto.FileName);

                            // Unikal fayl adı yaradırıq:
                            var uniqueFileName = $"{fileNameWithoutExt}_{Guid.NewGuid()}{extension}";

                            // Faylı serverdə saxlayırıq:
                            Foto.SaveAs(System.IO.Path.Combine(directory, uniqueFileName));

                            // Kitab obyektinə fayl adını əlavə edirik:
                            kitab.Foto = uniqueFileName;
                        }

                        var emeliyyatNeticesi = kitabManager.Elave(kitab); // Repositorydə Elave metodu var
                        if (emeliyyatNeticesi > 0)
                        {
                            TempData["SuccessMessage"] = "Kitab uğurla əlavə olundu!";
                            return RedirectToAction("CreateKitab"); //Əməliyyat uğurlu olduqda Moddal açılır, OK basıldıqda kitabların siyahısına yönləndirir
                        }
                        else
                        {
                            ModelState.AddModelError("", "Kitab əlavə olunarkən xəta baş verdi!");
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                var msg = ex.InnerException?.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError("", "Xəta: " + msg);
            }

            kitabViewModel.MuellifList = new MultiSelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", kitabViewModel.sechilmishMuellifIDler);
            kitabViewModel.KateqoriyaList = new MultiSelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kitabViewModel.sechilmishKateqoriyaIDler);

            return View(kitabViewModel);
        }

        // GET: Admin/KitabIdaresi/EditKitab
        public ActionResult EditKitab(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Kitab kitab = kitabManager.GetAllByIncludes(k => k.Muellif, k => k.Kateqoriya)
                .FirstOrDefault(k => k.KitabID == id.Value);

            if (kitab == null)
                return HttpNotFound();

            // Kitabı ViewModel-ə map edirik:
            var vm = new KitabViewModel
            {
                KitabID = kitab.KitabID,
                KitabAdi = kitab.KitabAdi,
                Haqqinda = kitab.Haqqinda,
                IcareQiymeti = kitab.IcareQiymeti,
                StokdaVarmi = kitab.StokdaVarmi,
                Foto = kitab.Foto,

                // Burada array-ları doldururuq:
                sechilmishMuellifIDler = kitab.Muellif.Select(m => m.MuellifID).ToArray(),
                sechilmishKateqoriyaIDler = kitab.Kateqoriya.Select(k => k.KateqoriyaID).ToArray(),

                // MultiSelectList-ləri də həmin seçilmiş array ilə qururuq:
                MuellifList = new MultiSelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", kitab.Muellif.Select(m => m.MuellifID).ToArray()),
                KateqoriyaList = new MultiSelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kitab.Kateqoriya.Select(k => k.KateqoriyaID).ToArray())
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKitab(KitabViewModel kitabViewModel, HttpPostedFileBase Foto, bool? chbFotoSil)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Əgər validasiya uğursuzdursa, MultiSelectList-ləri yenidən doldurmaq lazımdır:
                    kitabViewModel.MuellifList = new MultiSelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", kitabViewModel.sechilmishMuellifIDler);
                    kitabViewModel.KateqoriyaList = new MultiSelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kitabViewModel.sechilmishKateqoriyaIDler);
                    return View(kitabViewModel);
                }

                // Daxil edilən kitab adı və müəllif kombinasiyasının mövcudluğunu yoxla:
                var movcudKitablar = kitabManager.GetAllByIncludes(k => k.Muellif)
                    .Where(k => k.KitabAdi.ToLower() == kitabViewModel.KitabAdi.ToLower()
                                && k.KitabID != kitabViewModel.KitabID) // özünü çıxırıq
                    .ToList();

                bool eyniVar = movcudKitablar.Any(k =>
                    k.Muellif.Any(m => (kitabViewModel.sechilmishMuellifIDler ?? new int[0]).Contains(m.MuellifID)));

                if (eyniVar)
                {
                    ModelState.AddModelError("", "Bu kitab həmin müəllif(lər) ilə artıq mövcuddur!");

                    // MultiSelect-ləri doldur ki, səhifə boş qalmasın:
                    kitabViewModel.MuellifList = new MultiSelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", kitabViewModel.sechilmishMuellifIDler);
                    kitabViewModel.KateqoriyaList = new MultiSelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kitabViewModel.sechilmishKateqoriyaIDler);
                    return View(kitabViewModel);
                }

                // Database-dəki kitabı gətir:
                var original = kitabManager.GetAllByIncludes(k => k.Muellif, k => k.Kateqoriya).FirstOrDefault(k => k.KitabID == kitabViewModel.KitabID);

                if (original == null)
                    return HttpNotFound();

                // Müqayisə et: heç bir dəyişiklik yoxdurmu?
                bool eyniKitabAdi = original.KitabAdi == kitabViewModel.KitabAdi;
                bool eyniHaqqinda = original.Haqqinda == kitabViewModel.Haqqinda;
                bool eyniIcareQiymeti = original.IcareQiymeti == kitabViewModel.IcareQiymeti;
                bool eyniStokdaVarmi = original.StokdaVarmi == kitabViewModel.StokdaVarmi;

                bool eyniFoto = true;

                // Əgər yeni foto yüklənibsə, mövcud foto dəyişib:
                if (Foto != null && Foto.ContentLength > 0)
                    eyniFoto = false;

                // Əgər "Foto sil" işarələnibsə, mövcud foto dəyişib:
                if (chbFotoSil == true)
                    eyniFoto = false;

                // Əgər həm yeni foto, həm silinmə yoxdursasa, mövcud foto dəyişməyib


                bool eyniMuellifler = original.Muellif.Select(m => m.MuellifID).OrderBy(x => x).SequenceEqual((kitabViewModel.sechilmishMuellifIDler ?? new int[0]).OrderBy(x => x));

                bool eyniKateqoriyalar = original.Kateqoriya.Select(k => k.KateqoriyaID).OrderBy(x => x).SequenceEqual((kitabViewModel.sechilmishKateqoriyaIDler ?? new int[0]).OrderBy(x => x));

                if (eyniKitabAdi && eyniHaqqinda && eyniIcareQiymeti && eyniStokdaVarmi && eyniFoto && eyniMuellifler && eyniKateqoriyalar)
                {
                    ModelState.AddModelError("", "Heç bir dəyişiklik edilməyib!");

                    // MultiSelectList-ləri yenidən doldur ki, səhifə boş gəlməsin:
                    kitabViewModel.MuellifList = new MultiSelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", kitabViewModel.sechilmishMuellifIDler);
                    kitabViewModel.KateqoriyaList = new MultiSelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kitabViewModel.sechilmishKateqoriyaIDler);
                    return View(kitabViewModel);
                }

                // Əsas yeniləməni buradan et:
                original.KitabAdi = kitabViewModel.KitabAdi;
                original.Haqqinda = kitabViewModel.Haqqinda;
                original.IcareQiymeti = kitabViewModel.IcareQiymeti;
                original.StokdaVarmi = kitabViewModel.StokdaVarmi;
                original.Foto = kitabViewModel.Foto;

                // Müəllif və kateqoriyaları yenilə:
                original.Muellif.Clear();
                if (kitabViewModel.sechilmishMuellifIDler != null)
                {
                    foreach (var mId in kitabViewModel.sechilmishMuellifIDler)
                    {
                        var muellif = kitabManager.context.Muellif.Find(mId);
                        if (muellif != null) original.Muellif.Add(muellif);
                    }
                }

                original.Kateqoriya.Clear();
                if (kitabViewModel.sechilmishKateqoriyaIDler != null)
                {
                    foreach (var kId in kitabViewModel.sechilmishKateqoriyaIDler)
                    {
                        var kateqoriya = kitabManager.context.Kateqoriya.Find(kId);
                        if (kateqoriya != null) original.Kateqoriya.Add(kateqoriya);
                    }
                }

                // Mövcud foto silinməsi:
                if (chbFotoSil == true)
                {
                    original.Foto = string.Empty;
                }

                //Yeni foto əlavə et:
                if (Foto != null && Foto.ContentLength > 0)
                {
                    // Serverdə Images qovluğu yolunu müəyyən edirik:
                    string directory = Server.MapPath("~/Images/");

                    // Qovluq yoxlanır, yoxdursa yaradılır:
                    if (!System.IO.Directory.Exists(directory))
                        System.IO.Directory.CreateDirectory(directory);

                    // Orijinal fayl adını və extension-ı alırıq:
                    var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(Foto.FileName);
                    var extension = System.IO.Path.GetExtension(Foto.FileName);

                    // Unikal fayl adı yaradırıq:
                    var uniqueFileName = $"{fileNameWithoutExt}_{Guid.NewGuid()}{extension}";

                    // Faylı serverdə saxlayırıq:
                    var filePath = System.IO.Path.Combine(directory, uniqueFileName);
                    Foto.SaveAs(filePath);

                    // Kitab obyektinə fayl adını əlavə edirik:
                    original.Foto = uniqueFileName;
                }

                var emeliyyatNeticesi = kitabManager.Redakte(original, original.KitabID);
                if (emeliyyatNeticesi > 0)
                    TempData["SuccessMessage"] = "Kitab uğurla redaktə olundu!";

                return RedirectToAction("EditKitab", new { id = kitabViewModel.KitabID }); //id olmadan çağırsa səhv atacaq, burada id-ni ötürmək lazımdır
            }
            catch (Exception ex)
            {
                // Log faylı yolu:  ?
                string logPath = Server.MapPath("~/App_Data/ErrorLog.txt");

                // Fayla yazmaq:  ?
                System.IO.File.AppendAllText(logPath, $"[{DateTime.Now}] Xəta: {ex.Message}\nStackTrace: {ex.StackTrace}\n\n");

                // Xətanı logla və istifadəçiyə mesaj göstər:
                ModelState.AddModelError("", "Xəta baş verdi, kitab redaktə olunmadı! Detallar: " + ex.Message);

                // MultiSelect-ləri yenidən doldururuq ki, səhv olanda boş qalmasın:
                kitabViewModel.MuellifList = new MultiSelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", kitabViewModel.sechilmishMuellifIDler);
                kitabViewModel.KateqoriyaList = new MultiSelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kitabViewModel.sechilmishKateqoriyaIDler);

                return View(kitabViewModel);
            }
        }

        public ActionResult DeleteKitab(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Kitab kitab = kitabManager.GetAllByIncludes(k => k.Muellif, k => k.Kateqoriya).FirstOrDefault(k => k.KitabID == id.Value);

            if (kitab == null)
                return HttpNotFound();

            // Kitabı ViewModel-ə map edirik:
            var vm = new KitabViewModel
            {
                KitabID = kitab.KitabID,
                KitabAdi = kitab.KitabAdi,
                Haqqinda = kitab.Haqqinda,
                IcareQiymeti = kitab.IcareQiymeti,
                StokdaVarmi = kitab.StokdaVarmi,
                Foto = kitab.Foto,

                // Burada array-ları doldururuq:
                sechilmishMuellifIDler = kitab.Muellif.Select(m => m.MuellifID).ToArray(),
                sechilmishKateqoriyaIDler = kitab.Kateqoriya.Select(k => k.KateqoriyaID).ToArray(),

                // Burada adları birbaşa string kimi əlavə edirik:
                sechilmishMuellifAdi = string.Join(", ", kitab.Muellif.Select(m => m.MuellifAdSoyadi)),
                sechilmishKateqoriyaAdi = string.Join(", ", kitab.Kateqoriya.Select(k => k.KateqoriyaAdi))
            };

            return View(vm);
        }

        [HttpPost, ActionName("DeleteKitab")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Kitabı müəllif və kateqoriyalarla birlikdə gətiririk:
                var kitab = kitabManager.GetAllByIncludes(k => k.Muellif, k => k.Kateqoriya)
                                        .FirstOrDefault(k => k.KitabID == id);

                if (kitab == null)
                    return HttpNotFound();

                // Navigation-ları təmizləyirik ki, many-to-many əlaqələr problem yaratmasın:
                kitab.Muellif.Clear();
                kitab.Kateqoriya.Clear();

                // Kitabı sil:
                var emeliyyatNeticesi = kitabManager.Delete(id);
                if (emeliyyatNeticesi > 0)
                {
                    TempData["SuccessMessage"] = "Kitab uğurla silindi!";
                    return RedirectToAction("IndexKitab");
                }
                else
                {
                    ModelState.AddModelError("", "Kitab silinərkən xəta baş verdi!");
                }

                // Əgər səhv olarsa, yenidən kitabı göstərə bilərik:
                return View(kitab);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Xəta baş verdi, kitab silinmədi! Kitabın bağlı olduğu digər məlumatlar ola bilər.";
            }
            /*catch (Exception ex)
            {
                // Log faylı yolu:  ?
                string logPath = Server.MapPath("~/App_Data/ErrorLog.txt");

                // Fayla yazmaq:  ?
                System.IO.File.AppendAllText(logPath, $"[{DateTime.Now}] Xəta: {ex.Message}\nStackTrace: {ex.StackTrace}\n\n");

                ModelState.AddModelError("", "Xəta baş verdi, kitab silinmədi! Detallar: " + ex.Message);

                // Yenidən kitabı göstərə bilərik ki, səhifə boş qalmasın:
                var kitab = kitabManager.GetAllByIncludes(k => k.Muellif, k => k.Kateqoriya).FirstOrDefault(k => k.KitabID == id);

                return View(kitab);
            }*/
            return RedirectToAction("IndexKitab");
        }
    }
}
