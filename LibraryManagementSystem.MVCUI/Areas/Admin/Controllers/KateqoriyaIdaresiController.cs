using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class KateqoriyaIdaresiController : Controller
    {
        //private LibraryManagementSystemDBEntities db = new LibraryManagementSystemDBEntities();
        KateqoriyaManager kateqoriyaManager = new KateqoriyaManager();

        // GET: Admin/KateqoriyaIdaresi
        public ActionResult IndexKateqoriya(string searchText)
        {
            var kateqoriya = kateqoriyaManager.GetAll();

            if (!string.IsNullOrEmpty(searchText))
            {
                kateqoriya = kateqoriya
                    .Where(k => k.KateqoriyaAdi.Contains(searchText))
                    .ToList();
            }

            return View(kateqoriya);
        }

        // GET: Admin/KateqoriyaIdaresi/DetailsKateqoriya/5
        public ActionResult DetailsKateqoriya(int? id)
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
        public ActionResult CreateKateqoriya([Bind(Include = "KateqoriyaID,KateqoriyaAdi,KateqoriyaTesviri")] Kateqoriya kateqoriya)
        {
            if (ModelState.IsValid)
            {
                kateqoriyaManager.Add(kateqoriya);
                return RedirectToAction("IndexKateqoriya");
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKateqoriya([Bind(Include = "KateqoriyaID,KateqoriyaAdi,KateqoriyaTesviri")] Kateqoriya kateqoriya)
        {
            if (ModelState.IsValid)
            {
                kateqoriyaManager.Update(kateqoriya);
                return RedirectToAction("IndexKateqoriya");
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
        public ActionResult DeleteConfirmed(int id)
        {
            Kateqoriya kateqoriya = kateqoriyaManager.FindById(id);
            kateqoriyaManager.Delete(id);
            //kateqoriyaManager.Delete(kateqoriya.KateqoriyaID);
            return RedirectToAction("IndexKateqoriya");
        }
    }
}
