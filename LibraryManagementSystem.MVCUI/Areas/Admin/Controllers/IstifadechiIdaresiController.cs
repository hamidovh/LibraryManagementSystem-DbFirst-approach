using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class IstifadechiIdaresiController : BaseController
    {
        IstifadechiManager istifadechiManager = new IstifadechiManager();
        RolManager rolManager = new RolManager();

        // GET: Admin/IstifadechiIdaresi
        public ActionResult IndexIstifadechi(string searchText, string sortColumn, string sortOrder, string filterValue)
        {
            var istifadechi = istifadechiManager.GetAll();

            var rollar = rolManager.GetAll().Select(r => r.RolAdi).Distinct().ToList();
            ViewBag.Rollar = rollar;

            // Axtarış filteri:
            if (!string.IsNullOrEmpty(searchText))
            {
                string lowerSearch = searchText.ToLower();
                istifadechi = istifadechi
                    .Where(i =>
                        (i.Adi != null && i.Adi.ToLower().Contains(lowerSearch)) ||
                        (i.Soyadi != null && i.Soyadi.ToLower().Contains(lowerSearch)) ||
                        (i.Email != null && i.Email.ToLower().Contains(lowerSearch)) ||
                        (i.IstifadechiAdi != null && i.IstifadechiAdi.ToLower().Contains(lowerSearch)) ||
                        (i.Rol != null && i.Rol.RolAdi.ToLower().Contains(lowerSearch))
                    ).ToList();
            }

            // Sıralama və Filter:
            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn)
                {
                    case "Adi":
                        istifadechi = (sortOrder == "asc")
                            ? istifadechi.OrderBy(i => i.Adi).ToList()
                            : (sortOrder == "desc")
                                ? istifadechi.OrderByDescending(i => i.Adi).ToList()
                                : istifadechi.OrderBy(i => i.IstifadechiID).ToList();
                        break;

                    case "Soyadi":
                        istifadechi = (sortOrder == "asc")
                            ? istifadechi.OrderBy(i => i.Soyadi).ToList()
                            : (sortOrder == "desc")
                                ? istifadechi.OrderByDescending(i => i.Soyadi).ToList()
                                : istifadechi.OrderBy(i => i.IstifadechiID).ToList();
                        break;

                    case "IstifadechiAdi":
                        istifadechi = (sortOrder == "asc")
                            ? istifadechi.OrderBy(i => i.IstifadechiAdi).ToList()
                            : (sortOrder == "desc")
                                ? istifadechi.OrderByDescending(i => i.IstifadechiAdi).ToList()
                                : istifadechi.OrderBy(i => i.IstifadechiID).ToList();
                        break;

                    case "DoghumTarixi":
                        istifadechi = (sortOrder == "asc") // qocadan-cavana
                            ? istifadechi.OrderBy(i => i.DoghumTarixi).ToList()
                            : (sortOrder == "desc") // cavandan-qocaya
                                ? istifadechi.OrderByDescending(i => i.DoghumTarixi).ToList()
                                : istifadechi.OrderBy(i => i.IstifadechiID).ToList();
                        break;

                    case "Cins":
                        if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                        {
                            if (filterValue == "Kişi")
                                istifadechi = istifadechi.Where(i => i.Cins == "Kişi").ToList();
                            else if (filterValue == "Qadın")
                                istifadechi = istifadechi.Where(i => i.Cins == "Qadın").ToList();
                        }
                        break;

                    case "Email":
                        if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                        {
                            istifadechi = istifadechi
                                .Where(i => i.Email != null && i.Email.EndsWith(filterValue))
                                .ToList();
                        }
                        break;

                    case "TelefonNo":
                        if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                        {
                            istifadechi = istifadechi
                                .Where(i => i.TelefonNo != null && i.TelefonNo.StartsWith(filterValue))
                                .ToList();
                        }
                        break;

                    case "Aktivdirmi":
                        if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                        {
                            if (filterValue == "Aktivlər")
                                istifadechi = istifadechi.Where(i => i.Aktivdirmi == true).ToList();
                            else if (filterValue == "Deaktivlər")
                                istifadechi = istifadechi.Where(i => i.Aktivdirmi == false).ToList();
                        }
                        break;

                    case "QeydiyyatTarixi":
                        istifadechi = (sortOrder == "asc") // köhnədən-yeniyə
                            ? istifadechi.OrderBy(i => i.QeydiyyatTarixi).ToList()
                            : (sortOrder == "desc") // yenidən-köhnəyə
                                ? istifadechi.OrderByDescending(i => i.QeydiyyatTarixi).ToList()
                                : istifadechi.OrderBy(i => i.IstifadechiID).ToList();
                        break;

                    case "Rol":
                        if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                        {
                            istifadechi = istifadechi.Where(i => i.Rol.RolAdi == filterValue).ToList();
                        }
                        break;

                    case "Adres":
                        if (!string.IsNullOrEmpty(filterValue) && filterValue != "Hamısı")
                        {
                            istifadechi = istifadechi
                                .Where(i => i.Adres != null && i.Adres.Contains(filterValue))
                                .ToList();
                        }
                        break;
                    default:
                        break;
                }
            }

            ViewBag.CurrentSortColumn = sortColumn;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SelectedAdi = (sortColumn == "Adi") ? sortOrder : "";
            ViewBag.SelectedSoyadi = (sortColumn == "Soyadi") ? sortOrder : "";
            ViewBag.SelectedIstifadechiAdi = (sortColumn == "IstifadechiAdi") ? sortOrder : "";
            ViewBag.SelectedCins = (sortColumn == "Cins") ? filterValue : "";
            ViewBag.SelectedEmail = (sortColumn == "Email") ? filterValue : "";
            ViewBag.SelectedTelefonNo = (sortColumn == "TelefonNo") ? filterValue : "";
            ViewBag.SelectedDoghumTarixi = (sortColumn == "DoghumTarixi") ? sortOrder : "";
            ViewBag.SelectedQeydiyyatTarixi = (sortColumn == "QeydiyyatTarixi") ? sortOrder : "";
            ViewBag.SelectedAktivdirmi = (sortColumn == "Aktivdirmi") ? filterValue : "";
            ViewBag.SelectedAdres = (sortColumn == "Adres") ? filterValue : "";
            ViewBag.SelectedRol = (sortColumn == "Rol") ? filterValue : "";

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
                // Təkrar istifadəçi Adı yoxlanışı:
                if (istifadechiManager.GetAll().Any(i => i.IstifadechiAdi == istifadechi.IstifadechiAdi))
                {
                    ModelState.AddModelError("IstifadechiAdi", "Bu istifadəçi adı artıq mövcuddur!");
                }

                // Təkrar FİN kod yoxlanışı:
                if (istifadechiManager.GetAll().Any(i => i.FinKod == istifadechi.FinKod))
                {
                    ModelState.AddModelError("FinKod", "Bu FİN ilə qeydiyyat artıq bir dəfə həyata keçirilib!");
                }

                if (ModelState.IsValid)
                {
                    istifadechi.QeydiyyatTarixi = DateTime.Now;
                    var emeliyyatNeticesi = istifadechiManager.Add(istifadechi);

                    if (emeliyyatNeticesi > 0)
                    {
                        // TempData ilə mesajı göndər:
                        TempData["SuccessMessage"] = "İstifadəçi uğurla əlavə olundu!";
                        return RedirectToAction("CreateIstifadechi"); // uğurlu olduqda OK kliki ilə siyahıya qayıdır
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

            // Əgər səhv varsa, yenidən rolları doldurmaq lazımdır:
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

            ViewBag.RolID = new SelectList(rolManager.GetAll(), "RolID", "RolAdi", istifadechi?.RolID);

            return View(istifadechi);
        }

        // POST: Admin/IstifadechiIdaresi/EditIstifadechi/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIstifadechi(Istifadechi istifadechi)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Database-də var olan istifadəçini al:
                    var original = istifadechiManager.FindById(istifadechi.IstifadechiID);

                    if (original == null)
                    {
                        ModelState.AddModelError("", "İstifadəçi tapılmadı!");
                        return View(istifadechi);
                    }

                    // Əgər heç bir dəyişiklik edilməyibsə:
                    if (original.Adi == istifadechi.Adi &&
                        original.Soyadi == istifadechi.Soyadi &&
                        original.IstifadechiAdi == istifadechi.IstifadechiAdi &&
                        original.Email == istifadechi.Email &&
                        original.TelefonNo == istifadechi.TelefonNo &&
                        original.Cins == istifadechi.Cins &&
                        original.RolID == istifadechi.RolID &&
                        original.Aktivdirmi == istifadechi.Aktivdirmi &&
                        original.Adres == istifadechi.Adres &&
                        original.DoghumTarixi == istifadechi.DoghumTarixi &&
                        original.QeydiyyatTarixi == istifadechi.QeydiyyatTarixi &&
                        original.Shifre == istifadechi.Shifre)
                    {
                        ModelState.AddModelError("", "Heç bir dəyişiklik edilməyib!");
                        ViewBag.RolID = new SelectList(rolManager.GetAll(), "RolID", "RolAdi", istifadechi?.RolID);
                        return View(istifadechi);
                    }

                    // Dəyişikliklər varsa update et:
                    istifadechiManager.Update(istifadechi);

                    TempData["SuccessMessage"] = "Dəyişikliklər uğurla əlavə olundu!";
                    return RedirectToAction("EditIstifadechi", new { id = istifadechi.IstifadechiID });
                }

                ViewBag.RolID = new SelectList(rolManager.GetAll(), "RolID", "RolAdi", istifadechi?.RolID);
                return View(istifadechi);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Xəta baş verdi: " + ex.Message);
                return View(istifadechi);
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
        [HttpPost, ActionName("DeleteIstifadechi")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            try
            {
                Istifadechi istifadechi = istifadechiManager.FindById(id.Value);

                if (istifadechi != null)
                    istifadechiManager.Delete(istifadechi.IstifadechiID);

                TempData["SuccessMessage"] = "İstifadəçi uğurla silindi!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Xəta baş verdi!";
            }

            return RedirectToAction("IndexIstifadechi");
        }
    }
}
