using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.Models
{
    public class RespuestaUsuarios: Respuesta
    {
        public List<UserInformation> Data { get; set; }
    }
}