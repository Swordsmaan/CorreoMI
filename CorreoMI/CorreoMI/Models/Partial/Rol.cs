using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorreoMI.Models
{
    [MetadataType(typeof(RolMetadata))]
    public partial class Rol
    {
        public IEnumerable<SelectListItem> SelectedFuncionalidad { get; set; }
        public IEnumerable<SelectListItem> AvailableFuncionalidad { get; set; }
        public string[] PostedFuncionalidad { get; set; }

    }
}