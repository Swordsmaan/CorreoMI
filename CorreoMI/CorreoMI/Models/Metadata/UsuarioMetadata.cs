using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorreoMI.Models
{
    public class UsuarioMetadata
    {
        [Required(ErrorMessage = "El nombre no puede estar vacio")]
        [Display(Name = "Nombre")]
        [DataType(DataType.Text)]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "El Nombre solo debe contener letras, espacios y tener menos de 50 caracteres de largo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido no puede estar vacio")]
        [Display(Name = "Apellido")]
        [DataType(DataType.Text)]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "El Apellido solo debe contener letras, espacios y tener menos de 50 caracteres de largo")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El email no puede estar vacio")]
        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        [StringLength(320)]
        [RegularExpression("^[a-zA-Z0-9.@_-]+", ErrorMessage = "El Email solo debe contener letras, numeros, simbolos y tener hasta 320 caracteres de largo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La password no puede estar vacia")]
        [Display(Name = "Password")]
        [DataType(DataType.Text)]
        [StringLength(15)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "La Password solo debe contener letras, numeros y tener hasta 15 caracteres de largo")]
        public string Password { get; set; }

    }
}
