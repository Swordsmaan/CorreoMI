using EPPlusEnumerable;
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
    public class UsuarioController : Controller
    {
        private BDDMailEntities db = new BDDMailEntities();

        [Authorize(Roles = AuthConfig.Permission.CU007)]
        // GET: Usuario
        public ActionResult Index()
        {
            var usuario = db.Usuario.Include(u => u.Rol);
            return View(usuario.ToList());
        }

        [Authorize(Roles = AuthConfig.Permission.CU006)]
        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        [Authorize(Roles = AuthConfig.Permission.CU003)]
        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Usuario/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsuarioId,Nombre,Apellido,Email,Password,RolId")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.RolId = 5; // RolId = 5 => Rol de Pendiente
                if (db.Usuario.Any(a => a.Email == usuario.Email))
                {
                    //Agregar el mensaje de error "El email que intenta ingresar ya existe"
                    return View();
                }
                else
                {
                    db.Usuario.Add(usuario);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        [Authorize(Roles = AuthConfig.Permission.CU005)]
        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            if(usuario.RolId == 5) //Si el usuario tiene rol pendiente entonces no se puede editar hasta que no posea otro rol
            {
                return RedirectToAction("Index");
            }
            ViewBag.RolId = new SelectList(db.Rol, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsuarioId,Nombre,Apellido,Email,Password,RolId")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RolId = new SelectList(db.Rol, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        [Authorize(Roles = AuthConfig.Permission.CU004)]
        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            db.Usuario.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}