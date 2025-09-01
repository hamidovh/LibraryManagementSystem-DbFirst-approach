using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class KitabIdaresiController : Controller
    {
        KitabManager kitabManager = new KitabManager();
        MuellifManager muellifManager = new MuellifManager();
        KateqoriyaManager kateqoriyaManager = new KateqoriyaManager();

        // GET: Admin/KitabIdaresi
        public ActionResult IndexKitab(string searchText, int? muellifId, int? kateqoriyaId)
        {
            var kitab = kitabManager.GetAll();

            if (!string.IsNullOrEmpty(searchText))
            {
                kitab = kitab.Where(k =>
                    k.KitabAdi.Contains(searchText) ||
                    (k.Muellif != null && k.Muellif.MuellifAdSoyadi.Contains(searchText)) ||
                    (k.Kateqoriya != null && k.Kateqoriya.KateqoriyaAdi.Contains(searchText))
                ).ToList();
            }

            if (muellifId.HasValue)
            {
                kitab = kitab.Where(k => k.MuellifID == muellifId.Value).ToList();
            }

            if (kateqoriyaId.HasValue)
            {
                kitab = kitab.Where(k => k.KateqoriyaID == kateqoriyaId.Value).ToList();
            }

            ViewBag.MuellifID = new SelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", muellifId);
            ViewBag.KateqoriyaID = new SelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kateqoriyaId);

            return View(kitab);
        }

        public ActionResult CreateKitab()
        {
            ViewBag.MuellifID = new SelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi");
            ViewBag.KateqoriyaID = new SelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi");

            return View();
        }

        [HttpPost]
        public ActionResult CreateKitab(Kitab kitab)
        {
            try
            {
                if (ModelState.IsValid) //Boş qalmamalı olan EditorForların dolu gəlib-gəlmədiyini yoxlayır
                {
                    var emeliyyatNeticesi = kitabManager.Add(kitab);
                    if (emeliyyatNeticesi > 0)
                    {
                        return RedirectToAction("IndexKitab"); //Əməliyyat uğurlu olduqda kitabların siyahısına yönləndirir
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kitab əlavə olunarkən xəta baş verdi!");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Xəta baş verdi, kitab əlavə olunmadı!");
            }

            ViewBag.MuellifID = new SelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi");
            ViewBag.KateqoriyaID = new SelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi");

            return View();
        }

        public ActionResult EditKitab(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Kitab kitab = kitabManager.FindById(id.Value);
            if (kitab == null)
            {
                return HttpNotFound();
            }

            ViewBag.MuellifID = new SelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", kitab.MuellifID);
            ViewBag.KateqoriyaID = new SelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kitab.KateqoriyaID);

            return View(kitab);
        }

        [HttpPost]
        public ActionResult EditKitab(Kitab kitab)
        {
            try
            {
                if (ModelState.IsValid) //Boş qalmamalı olan EditorForların dolu gəlib-gəlmədiyini yoxlayır
                {
                    var emeliyyatNeticesi = kitabManager.Update(kitab);
                    if (emeliyyatNeticesi > 0)
                    {
                        return RedirectToAction("IndexKitab"); //Əməliyyat uğurlu olduqda kitabların siyahısına yönləndirir
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Xəta baş verdi, kitab redaktə olunmadı!");
            }

            ViewBag.MuellifID = new SelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", kitab.MuellifID);
            ViewBag.KateqoriyaID = new SelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kitab.KateqoriyaID);

            return View(kitab);
        }

        public ActionResult DeleteKitab(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Kitab kitab = kitabManager.FindById(id.Value);
            if (kitab == null)
            {
                return HttpNotFound();
            }

            ViewBag.MuellifID = new SelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", kitab.MuellifID);
            ViewBag.KateqoriyaID = new SelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kitab.KateqoriyaID);

            return View(kitab);
        }

        [HttpPost]
        public ActionResult DeleteKitab(int? id, string test)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Kitab kitab = kitabManager.FindById(id.Value);
            if (kitab == null)
            {
                return HttpNotFound();
            }

            try
            {
                kitabManager.Delete(id.Value);
                return RedirectToAction("IndexKitab");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Xəta baş verdi, kitab silinmədi!");
            }

            ViewBag.MuellifID = new SelectList(muellifManager.GetAll(), "MuellifID", "MuellifAdSoyadi", kitab.MuellifID);
            ViewBag.KateqoriyaID = new SelectList(kateqoriyaManager.GetAll(), "KateqoriyaID", "KateqoriyaAdi", kitab.KateqoriyaID);

            return View(kitab);
        }
    }
}
