using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.Models
{
    public class RespuestaUserInfo: Respuesta
    {
        public UserInformation Data { get; set; }
    }
}