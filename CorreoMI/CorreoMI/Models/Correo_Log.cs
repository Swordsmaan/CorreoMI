//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CorreoMI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Correo_Log
    {
        public int CorreoId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Nullable<int> UsuarioId { get; set; }
    
        public virtual Usuario Usuario { get; set; }
    }
}
