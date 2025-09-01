using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class MuellifIdaresiController : Controller
    {
        //private LibraryManagementSystemDBEntities db = new LibraryManagementSystemDBEntities();
        MuellifManager muellifManager = new MuellifManager();

        // GET: Admin/MuellifIdaresi
        public ActionResult IndexMuellif(string searchText)
        {
            var muellif = muellifManager.GetAll();

            if (!string.IsNullOrEmpty(searchText))
            {
                muellif = muellif.Where(m =>
                    m.MuellifAdi.Contains(searchText) ||
                    m.MuellifSoyadi.Contains(searchText)
                ).ToList();
            }

            return View(muellif);
        }

        // GET: Admin/MuellifIdaresi/Details/5
        public ActionResult DetailsMuellif(int? id)
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
        public ActionResult CreateMuellif([Bind(Include = "MuellifID,MuellifAdi,MuellifSoyadi,MuellifinDoghumTarixi")] Muellif muellif)
        {
            if (ModelState.IsValid)
            {
                muellifManager.Add(muellif);
                //muellifManager.Save();
                return RedirectToAction("IndexMuellif");
            }

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
        public ActionResult EditMuellif([Bind(Include = "MuellifID,MuellifAdi,MuellifSoyadi,MuellifinDoghumTarixi")] Muellif muellif)
        {
            if (ModelState.IsValid)
            {
                muellifManager.Update(muellif);
                return RedirectToAction("IndexMuellif");
            }
            return View(muellif);
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
            Muellif muellif = muellifManager.FindById(id);
            muellifManager.Delete(muellif.MuellifID);
            return RedirectToAction("IndexMuellif");
        } 
    }
}
