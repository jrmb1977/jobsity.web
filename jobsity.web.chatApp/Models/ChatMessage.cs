using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Message { get; set; }

        public ChatMessage()
        {
            Id = 0;
            Sender = "";
            Receiver = "";
            Message = "";
        }

        public ChatMessage(DataRow RegUsr)
        {
            var columnas = RegUsr.Table.Columns;

            int intCodigo = 0;

            if (columnas.Contains("Id"))
            {
                intCodigo = 0;
                int.TryParse(RegUsr["Id"].ToString(), out intCodigo);
                this.Id = intCodigo;
            }

            if (columnas.Contains("Usuario"))
                this.Sender = RegUsr["Usuario"].ToString();

            if (columnas.Contains("Message"))
                this.Message = RegUsr["Message"].ToString();
        }

        public bool IsEmpty()
        {
            bool Empty = string.IsNullOrEmpty(Message);
            return Empty;
        }
    }
}