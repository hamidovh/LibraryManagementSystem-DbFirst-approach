using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.MVCUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.User.Controllers
{
    // User Area HomeController.cs (~/Areas/User/Controllers/HomeController)::
    public class HomeController : Controller
    {
        KitabManager kitabManager = new KitabManager();
        SliderManager sliderManager = new SliderManager();

        // GET: User/Home
        [Authorize] // yalnız login olmuş istifadəçilər baxa bilər
        public ActionResult Index(int page = 1)
        {
            // İstifadəçi məlumatlarını Session və ya DB-dən çəkir:

            int pageSize = 10; // istəyə görə
            var model = new EsasSehifeVM
            {
                Kitablar = kitabManager.GetPaged(page, pageSize, k => k.KitabID).ToList(),
                Sliders = sliderManager.GetAll(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)kitabManager.GetAll().Count() / pageSize)
            };

            return View(model);
        }
    }
}
