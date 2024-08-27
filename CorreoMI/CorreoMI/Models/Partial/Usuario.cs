using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorreoMI.Models
{
    [MetadataType(typeof(UsuarioMetadata))]
    public partial class Usuario
    {
        public string DisplayName
        {
            get
            {
                return this.Nombre + " " + this.Apellido + " (" +this.Rol.Nombre+")".ToString();

            }
        }
    }
}