using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using jobsity.web.chatApp.App_Code;
using jobsity.web.chatApp.Models;
using Microsoft.AspNet.Identity;

namespace jobsity.web.chatApp.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult sendmsg(string message)
        {
            string sender = User.Identity.GetUserName();
            RespuestaPublish response = Data.Publish(sender, message);
            return Json(response);
        }

        [HttpPost]
        public JsonResult receivemsg()
        {
            string receiver = User.Identity.GetUserName();
            RespuestaPublish response = Data.Suscribe(receiver);
            // 1. Se Inserta a la BBDD los Mensajes extraidos de la cola
            if (response.EsOK())
            {
                ChatMessage message = response.Data;
                if (!message.IsEmpty())
                {
                    Respuesta respuestaInsert = Data.InsertMessageInChatRoom(message);
                }
            }
            // 2. Se extraen los Mensajes no leidos aun de la Base de Datos
            RespuestaMessages respuesta = Data.GetNotReadMessages(receiver);
            return Json(respuesta);
        }
    }
}