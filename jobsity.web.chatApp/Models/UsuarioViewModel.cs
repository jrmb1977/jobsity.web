using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.Models
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        // Only contains alphanumeric characters (lowercase letters and digits) and not underscore and not dot .
        // username is 6-25 characters long (?=.{6,25}$)
        // no digit at the beginning (?![0-9_])
        // no __ inside (?!.*[_]{2})
        // allowed characters [a-z0-9_]+
        // no _ at the end (?<![_])
        [DisplayName("Usuario")]
        [Required, MaxLength(25)]
        [RegularExpression("^(?=.{6,25}$)(?![0-9])[a-z0-9]+$", ErrorMessage = "Formato NO es válido")]
        public string Usuario { get; set; }

        [DisplayName("Nombre")]
        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [DisplayName("Código")]
        [Required, MaxLength(20)]
        public string Codigo { get; set; }

        [DisplayName("Descripción")]
        [Required, MaxLength(100)]
        public string Descripcion { get; set; }

        [DisplayName("Correo")]
        [Required, MaxLength(320)]
        [EmailAddress(ErrorMessage = "Formato NO es válido")]
        public string Correo { get; set; }
    }
}