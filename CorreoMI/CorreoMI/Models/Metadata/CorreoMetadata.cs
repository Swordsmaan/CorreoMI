using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorreoMI.Models
{
    public class CorreoMetadata
    {
        [Required(ErrorMessage = "El asunto no puede estar vacio")]
        [Display(Name = "Asunto")]
        [DataType(DataType.Text)]
        [StringLength(200)]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "El Asunto solo debe contener letras, espacios y tener menos de 200 caracteres de largo")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "El mensaje no puede estar vacio")]
        [Display(Name = "Mensaje")]
        [DataType(DataType.Text)]
        [StringLength(8000)]
        [RegularExpression("^[a-zA-Z0-9]+@*$", ErrorMessage = "El Mensaje solo debe contener letras, espacios y tener menos de 50 caracteres de largo")]
        public string Message { get; set; }

    }
}
