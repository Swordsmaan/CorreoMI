using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CorreoMI.Models;

namespace CorreoMI.Controllers
{
    public class FuncionalidadController : Controller
    {
        private BDDMailEntities db = new BDDMailEntities();

        [Authorize(Roles = AuthConfig.Permission.CU015)]
        // GET: Funcionalidad
        public ActionResult Index()
        {
            return View(db.Funcionalidad.ToList());
        }
    }
}