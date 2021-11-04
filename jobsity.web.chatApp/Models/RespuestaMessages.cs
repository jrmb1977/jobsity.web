using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.Models
{
    public class RespuestaMessages: Respuesta
    {
        public List<ChatMessage> Data { get; set; }
    }
}