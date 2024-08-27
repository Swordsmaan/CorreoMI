using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CorreoMI.Models;
using System.Net;
using System.Net.Mail;

namespace CorreoMI.Controllers
{
    public class LogonController : Controller
    {
        private BDDMailEntities db = new BDDMailEntities();

        // GET: Logon
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (AuthConfig.GetIsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Logon
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(string Email, string Password, string returnUrl)
        {
            //returnURL needs to be decoded
            string decodedUrl = "";
            if (!string.IsNullOrEmpty(returnUrl))
                decodedUrl = Server.UrlDecode(returnUrl);

            //TODO: your authentication logic 
            if (new AuthConfig().Authenticate(Email.ToLower(), Password))
            {
                FormsAuthentication.SetAuthCookie(Email.ToLower(), false);
            }
            else
            {
                return View();
            }

            if (Url.IsLocalUrl(decodedUrl))
            {
                return Redirect(decodedUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Logon");
        }

        //GET: Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UsuarioId,Nombre,Apellido,Email,Password,RolId")] Usuario usuario)
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
                    SendActivationEmail(usuario);
                }
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        //ACTIVACION DE MAIL////////////////////////////////////////////////////////

        public ActionResult Activation()
        {
            ViewBag.Message = "Codigo de activacion invalida."; //Si el codigo ya ha sido usado, entonces devuelve un mensaje diciendo que es invalido
            if (RouteData.Values["id"] != null)
            {
                Guid activationCode = new Guid(RouteData.Values["id"].ToString());
                UsuarioActivacion userActivation = db.UsuarioActivaciones.Where(p => p.ActivationCode == activationCode).FirstOrDefault();
                Usuario usuario = db.Usuario.First(f=>f.UsuarioId == userActivation.UsuarioId);
                if (userActivation != null)
                {
                    usuario.RolId = 6;
                    db.UsuarioActivaciones.Remove(userActivation);//Se elimina la activacion una vez que fue usada
                    db.SaveChanges();
                    ViewBag.Message = "Activacion exitosa.";
                }
            }
            return View();
        }

        //ENVIO DE CORREO//////////////////////////////////////////////////////////

        private void SendActivationEmail(Usuario usuario)
        {
            Guid activationCode = Guid.NewGuid();
            db.UsuarioActivaciones.Add(new UsuarioActivacion
            {
                UsuarioId = usuario.UsuarioId,
                ActivationCode = activationCode
            });
            db.SaveChanges();

            using (MailMessage mm = new MailMessage("appcorreomi2018@gmail.com", usuario.Email))
            {
                mm.Subject = "Activacion de cuenta";
                string body = "Hola " + usuario.Nombre + ",";
                body += "<br /><br />Por favor haga click en el siguiente link para activar su cuenta";
                body += "<br /><a href = '" + string.Format("{0}://{1}/Logon/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click aqui para activar su cuenta.</a>";
                body += "<br /><br />Gracias";
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("appcorreomi2018@gmail.com", "CorreoMI123");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
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