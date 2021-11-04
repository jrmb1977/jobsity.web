using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.Models
{
    public class SetPasswordViewModelApp
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Clave")]
        public string NuevaClave { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nueva Clave")]
        [Compare("NuevaClave", ErrorMessage = "No coinciden.")]
        public string ConfirmarClave { get; set; }
    }
}