using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.Models
{
    public class RespuestaPublish: Respuesta
    {
        public ChatMessage Data { get; set; }
    }
}