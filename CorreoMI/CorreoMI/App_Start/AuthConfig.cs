using System.Linq;
using CorreoMI.Models;
using System.Security.Cryptography;
using System.Text;
using System;

namespace CorreoMI
{
    public class AuthConfig
    {
        public static class Permission
        {
            //Lista de funcionalidades
            /// <summary>
            /// Ingreso al sistema
            /// </summary>
            public const string CU001 = nameof(CU001);

            /// <summary>
            /// Envio de mail
            /// </summary>
            public const string CU002 = nameof(CU002);
            
            /// <summary>
            /// Alta de usuario
            /// </summary>
            public const string CU003 = nameof(CU003);
            
            /// <summary>
            /// Baja de usuario
            /// </summary>
            public const string CU004 = nameof(CU004);
            
            /// <summary>
            /// Modificacion de usuario
            /// </summary>
            public const string CU005 = nameof(CU005);
            
            /// <summary>
            /// Detalle de usuario
            /// </summary>
            public const string CU006 = nameof(CU006);
            
            /// <summary>
            /// Listado de usuario
            /// </summary>
            public const string CU007 = nameof(CU007);

            /// <summary>
            /// Alta de rol
            /// </summary>
            public const string CU008 = nameof(CU008);

            /// <summary>
            /// Baja de rol
            /// </summary>
            public const string CU009 = nameof(CU009);

            /// <summary>
            /// Modificacion de rol
            /// </summary>
            public const string CU010 = nameof(CU010);

            /// <summary>
            /// Detalle de rol
            /// </summary>
            public const string CU011 = nameof(CU011);

            /// <summary>
            /// Listado de rol
            /// </summary>
            public const string CU012 = nameof(CU012);

            /// <summary>
            /// Detalle de mail
            /// </summary>
            public const string CU013 = nameof(CU013);

            /// <summary>
            /// Listado de mail
            /// </summary>
            public const string CU014 = nameof(CU014);

            /// <summary>
            /// Listado de funcionalidades
            /// </summary>
            public const string CU015 = nameof(CU015);
        }

        private BDDMailEntities db = new BDDMailEntities();

        public AuthConfig()
        {
        }

        //Autentifica el usuario y contraseña
        public bool Authenticate(string email, string password)
        {
            //Verifica si el usuario existe
            Usuario usuario = db.Usuario.Where(w => w.Email.ToLower() == email.ToLower()).FirstOrDefault();
            if (usuario != null) //Si el usuario no es null
            {
                if (usuario.Password == password) //Si la contraseña del usuario coincide
                {
                    return true; //lo autentifica
                }
                else //Si la contraseña no coincide
                {
                    return false; //no lo autentifica
                }
 
            }
            return false; //Si el usuario no existe no lo autentifica

        }

        public bool GetUserIsRol(string funcionalidad) //Devuelve un bool si la funcionalidad es igual o no a la comparada.
        {
            return db.Usuario
                .Any(w => w.Email == GetUser && w.Rol.Funcionalidad.Any(a => a.FuncionalidadId == funcionalidad));
        }

        public string[] GetUserRoles(string email) //Devuelve un array con el ID de cada funcionalidad
        {
            string[] result = db.Usuario
                .Where(w => w.Email == email)
                .SelectMany(s => s.Rol.Funcionalidad)
                .Select(s => s.FuncionalidadId)
                .ToArray();
            return result;
        }

        public string GetUserName //Devuelve el nombre del usuario
        {
            get
            {
                return db.Usuario
                    .Where(w => w.Email == GetUser)
                    .First()
                    .Nombre;
            }
        }


        public int GetUserId //Devuelve el id del usuario
        {
            get
            {
                return db.Usuario
                    .Where(w => w.Email == GetUser)
                    .First()
                    .UsuarioId;
            }
        }

        #region Static
        public static string GetUser { get { return System.Web.HttpContext.Current.User.Identity.Name; } }

        public static bool GetIsAuthenticated { get { return System.Web.HttpContext.Current.User.Identity.IsAuthenticated; } }
        #endregion
    }
}