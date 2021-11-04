using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.Models
{
    public class Respuesta
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }

        public Respuesta()
        {
            Codigo = 0;
            Mensaje = "OK";
        }

        public bool EsOK()
        {
            bool IsOK = Codigo.Equals(0);
            return IsOK;
        }
    }
}