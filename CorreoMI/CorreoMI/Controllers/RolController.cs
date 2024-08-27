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
    public class RolController : Controller
    {
        private BDDMailEntities db = new BDDMailEntities();

        [Authorize(Roles = AuthConfig.Permission.CU012)]
        // GET: Rol
        public ActionResult Index()
        {
            return View(db.Rol.ToList());
        }

        [Authorize(Roles = AuthConfig.Permission.CU011)]
        // GET: Rol/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = db.Rol.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        [Authorize(Roles = AuthConfig.Permission.CU008)]
        // GET: Rol/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rol/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RolId,Nombre,Default,Activo")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                db.Rol.Add(rol);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = rol.RolId });
            }
            return View(rol);
        }

        [Authorize(Roles = AuthConfig.Permission.CU010)]
        // GET: Rol/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = db.Rol.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            if (rol.Default)
            {
                return RedirectToAction("Index");
            }

            rol.AvailableFuncionalidad = db.Funcionalidad.Select(s => new SelectListItem { Value = s.FuncionalidadId, Text = s.Nombre }).ToList();
            rol.SelectedFuncionalidad = rol.Funcionalidad.Select(s => new SelectListItem { Value = s.FuncionalidadId, Text = s.Nombre }).ToList();
            foreach (var item in rol.AvailableFuncionalidad.Where(w => rol.SelectedFuncionalidad.Select(s => s.Value).Contains(w.Value)))
            {
                item.Selected = true;
            }
            return View(rol);
        }

        // POST: Rol/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RolId,Nombre,Default,Activo,PostedFuncionalidad")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                Rol rolAux = db.Rol.Find(rol.RolId);
                //Remover todas las funcionalidades
                foreach (var item in rolAux.Funcionalidad.ToList())
                {
                    rolAux.Funcionalidad.Remove(item);
                }
                db.SaveChanges();
                db.Entry(rolAux).State = EntityState.Modified;
                db.Entry(rolAux).State = EntityState.Detached;
                db.Entry(rol).State = EntityState.Modified;
                //Agregar las funcionalidades seleccionadas
                if (rol.PostedFuncionalidad != null)
                {
                    foreach (var item in rol.PostedFuncionalidad)
                    {
                        rol.Funcionalidad.Add(db.Funcionalidad.Find(item));
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            rol.AvailableFuncionalidad = db.Funcionalidad.Select(s => new SelectListItem { Value = s.FuncionalidadId }).ToList();
            rol.SelectedFuncionalidad = rol.Funcionalidad.Select(s => new SelectListItem { Value = s.FuncionalidadId }).ToList();
            foreach (var item in rol.AvailableFuncionalidad.Where(w => rol.SelectedFuncionalidad.Select(s => s.Value).Contains(w.Value)))
            {
                item.Selected = true;
            }
            return View(rol);
        }

        [Authorize(Roles = AuthConfig.Permission.CU009)]
        // GET: Rol/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = db.Rol.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            if (rol.Default)
            {
                return RedirectToAction("Index");
            }
            return View(rol);
        }

        // POST: Rol/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rol rol = db.Rol.Find(id);
            rol.AvailableFuncionalidad = db.Funcionalidad.Select(s => new SelectListItem { Value = s.FuncionalidadId }).ToList();
            rol.SelectedFuncionalidad = rol.Funcionalidad.Select(s => new SelectListItem { Value = s.FuncionalidadId }).ToList();
            foreach (var item in rol.AvailableFuncionalidad.Where(w => rol.SelectedFuncionalidad.Select(s => s.Value).Contains(w.Value)))
            {
                item.Selected = false;
            }
            db.Rol.Remove(rol);
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