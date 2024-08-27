using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorreoMI.Models;
using System.Net;
using System.Net.Mail;

namespace CorreoMI.Controllers
{
    public class MailController : Controller
    {
        private BDDMailEntities db = new BDDMailEntities();

        [Authorize(Roles = AuthConfig.Permission.CU014)]
        // GET: Mail/Index
        public ActionResult Index()
        {
            return View(db.Correo_Log.ToList());
        }

        [Authorize(Roles = AuthConfig.Permission.CU013)]
        // GET: Mail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Correo_Log correo = db.Correo_Log.Find(id);
            if (correo == null)
            {
                return HttpNotFound();
            }
            return View(correo);
        }

        [Authorize(Roles = AuthConfig.Permission.CU002)]
        // GET: Mail/Send
        public ActionResult Send()
        {
            
            ViewBag.To = new SelectList(db.Usuario, "Email", "DisplayName");
            return View();
        }

        // POST: Mail/Send
        [HttpPost]
        public ViewResult Send(Correo_Log _objModelMail)
        {
            if (ModelState.IsValid)
            {
                int id = new AuthConfig().GetUserId;
                Usuario u = db.Usuario.Find(id);
                _objModelMail.From = u.Email;

                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                string Body = "De: " + u.Email;
                Body += "<br />" + _objModelMail.Message;
                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //Cambiar por dominio usado
                smtp.Port = 587; //Cambiar por el puerto del dominio
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("appcorreomi2018@gmail.com", "CorreoMI123"); //Cambiar por mail y cuenta usados para envio de mail
                smtp.EnableSsl = true;
                smtp.Send(mail);
                db.Correo_Log.Add(_objModelMail);
                db.SaveChanges();
            }
           
            ViewBag.To = new SelectList(db.Usuario, "Email", "DisplayName", _objModelMail.To);
            return View();
        }
    }
}