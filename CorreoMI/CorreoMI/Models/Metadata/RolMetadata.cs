using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorreoMI.Models
{
    public class RolMetadata
    {
        [Required(ErrorMessage = "El nombre del rol no puede estar vacio")]
        [Display(Name = "Rol")]
        [DataType(DataType.Text)]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "El Nombre solo debe contener letras, espacios y tener menos de 50 caracteres de largo")]
        public string Nombre { get; set; }

    }
}
