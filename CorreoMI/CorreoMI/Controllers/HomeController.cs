using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorreoMI.Models;

namespace CorreoMI.Controllers
{
    public class HomeController : Controller
    {
        private BDDMailEntities db = new BDDMailEntities();

        // GET: Home
        public ActionResult Index()//Agregar int? id
        {
            return View();
        }
    }
}