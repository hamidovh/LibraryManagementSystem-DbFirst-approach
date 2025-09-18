using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.Controllers
{
    // Admin Area DefaultController.cs (~/Areas/Admin/Controllers/DefaultController):
    public class DefaultController : BaseController
    {
        // GET: Admin/Default
        public ActionResult Index()
        {
            return View();
        }
    }
}
