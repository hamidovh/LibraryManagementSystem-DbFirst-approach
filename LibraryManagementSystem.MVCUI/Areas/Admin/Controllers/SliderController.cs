using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.MVCUI.Models;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    public class SliderController : BaseController
    {
        SliderManager sliderManager = new SliderManager();

        // GET: Admin/Slider
        public ActionResult IndexSlider()
        {
            var slider = sliderManager.GetAll();
            return View(slider);
        }

        // GET: Admin/Slider/DetailsSlider/5
        //public ActionResult DetailsSlider(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Slider slider = db.Slider.Find(id);
        //    if (slider == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(slider);
        //}

        // GET: Admin/Slider/CreateSlider
        public ActionResult CreateSlider()
        {
            return View();
        }

        // POST: Admin/Slider/CreateSlider
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSlider(Slider slider, HttpPostedFileBase Sekil)
        {
            if (ModelState.IsValid)
            {
                //Eyni ID-li slider yoxdursa əlavə et:
                var movcudSlider = sliderManager.GetAll().FirstOrDefault(s => s.SliderID == slider.SliderID);
                if (movcudSlider != null)
                {
                    ModelState.AddModelError("", "Bu ID ilə artıq bir slider mövcuddur!");
                    return View(slider);
                }

                //Şəkil əlavə et:
                if (Sekil != null && Sekil.ContentLength > 0)
                {
                    // Serverdə Images qovluğu yolunu müəyyən edirik:
                    string directory = Server.MapPath("~/Images/");

                    // Qovluq yoxlanır, yoxdursa yaradılır:
                    if (!System.IO.Directory.Exists(directory))
                        System.IO.Directory.CreateDirectory(directory);

                    // Orijinal fayl adını və extension-ı alırıq:
                    var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(Sekil.FileName);
                    var extension = System.IO.Path.GetExtension(Sekil.FileName);

                    // Unikal fayl adı yaradırıq:
                    var uniqueFileName = $"{fileNameWithoutExt}_{Guid.NewGuid()}{extension}";

                    // Faylı serverdə saxlayırıq:
                    Sekil.SaveAs(System.IO.Path.Combine(directory, uniqueFileName));

                    // Kitab obyektinə fayl adını əlavə edirik:
                    slider.Sekil = uniqueFileName;
                }

                var emeliyyatNeticesi = sliderManager.Add(slider);
                if (emeliyyatNeticesi > 0)
                {
                    return RedirectToAction("IndexSlider");
                }
            }

            return View(slider);
        }

        // GET: Admin/Slider/EditSlider/5
        public ActionResult EditSlider(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = sliderManager.FindById(id.Value);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Slider/EditSlider/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSlider(Slider slider, HttpPostedFileBase Sekil)
        {
            if (ModelState.IsValid)
            {
                var movcudSlider = sliderManager.FindById(slider.SliderID);
                if (movcudSlider == null)
                {
                    ModelState.AddModelError("", "Bu ID ilə bir slider tapılmadı!");
                    return View(slider);
                }



                //Şəkil əlavə et:
                if (Sekil != null && Sekil.ContentLength > 0)
                {
                    // Serverdə Images qovluğu yolunu müəyyən edirik:
                    string directory = Server.MapPath("~/Images/");

                    // Qovluq yoxlanır, yoxdursa yaradılır:
                    if (!System.IO.Directory.Exists(directory))
                        System.IO.Directory.CreateDirectory(directory);

                    // Orijinal fayl adını və extension-ı alırıq:
                    var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(Sekil.FileName);
                    var extension = System.IO.Path.GetExtension(Sekil.FileName);

                    // Unikal fayl adı yaradırıq:
                    var uniqueFileName = $"{fileNameWithoutExt}_{Guid.NewGuid()}{extension}";

                    // Faylı serverdə saxlayırıq:
                    var filePath = System.IO.Path.Combine(directory, uniqueFileName);
                    Sekil.SaveAs(filePath);

                    // Kitab obyektinə fayl adını əlavə edirik:
                    slider.Sekil = uniqueFileName;
                }

                var emeliyyatNeticesi = sliderManager.Update(slider);
                if (emeliyyatNeticesi > 0)
                {
                    return RedirectToAction("IndexSlider");
                }
            }
            return View(slider);
        }

        // GET: Admin/Slider/DeleteSlider/5
        public ActionResult DeleteSlider(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = sliderManager.FindById(id.Value);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Slider/DeleteSlider/5
        [HttpPost, ActionName("DeleteSlider")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = sliderManager.FindById(id);
            var emeliyyatNeticesi = sliderManager.Delete(slider.SliderID);
            if (emeliyyatNeticesi > 0)
            {
                return RedirectToAction("IndexSlider");
            }
            return View(slider);
        }
    }
}
