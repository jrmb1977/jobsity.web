using jobsity.web.chatApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.App_Code
{
    public class Data
    {
        #region RabbitMQ

        public static RespuestaPublish Publish(string sender, string message)
        {
            int Codigo = 0;
            string Mensaje = "OK";
            ChatMessage chatMessage = new ChatMessage();
            chatMessage.Id = 1;
            chatMessage.Sender = sender;
            chatMessage.Receiver = "chat";
            chatMessage.Message = message;
            try
            {
                string jsonMessage = JsonConvert.SerializeObject(chatMessage);
                RabbitMQData oData = new RabbitMQData();
                bool response = oData.Send(jsonMessage);
            }
            catch (Exception exc)
            {
                Codigo = 9;
                Mensaje = exc.Message;
            }
            RespuestaPublish respuesta = new RespuestaPublish
            {
                Codigo = Codigo,
                Mensaje = Mensaje,
                Data = chatMessage
            };
            return respuesta;
        }

        public static RespuestaPublish Suscribe(string receiver)
        {
            int Codigo = 0;
            string Mensaje = "OK";
            ChatMessage chatMessage = new ChatMessage();
            try
            {
                RabbitMQData oData = new RabbitMQData();
                string JsonMessage = oData.Receive();
                if (!string.IsNullOrEmpty(JsonMessage))
                {
                    chatMessage = JsonConvert.DeserializeObject<ChatMessage>(JsonMessage);
                }
                chatMessage.Receiver = receiver;
            }
            catch (Exception exc)
            {
                Codigo = 9;
                Mensaje = exc.Message;
            }
            RespuestaPublish respuesta = new RespuestaPublish
            {
                Codigo = Codigo,
                Mensaje = Mensaje,
                Data = chatMessage
            };
            return respuesta;
        }

        #endregion

        #region Chat Database

        #region Chat Room

        public static RespuestaMessages GetNotReadMessages(string Usuario)
        {
            int codigo = 0;
            string mensaje = "OK";
            List<ChatMessage> msgs = new List<ChatMessage>();
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                DataTable dt = ChatDataBase.SelectNotReadMessages(ref oDataBase, Usuario);
                foreach (DataRow row in dt.Rows)
                {
                    ChatMessage msg = new ChatMessage(row);
                    msg.Receiver = Usuario;
                    ChatDataBase.InsertReadMessage(ref oDataBase, msg);
                    msgs.Add(msg);
                }
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            RespuestaMessages respuesta = new RespuestaMessages { 
                Codigo = codigo, 
                Mensaje = mensaje, 
                Data = msgs 
            };
            return respuesta;
        }

        public static Respuesta InsertMessageInChatRoom(ChatMessage message)
        {
            int codigo = 0;
            string mensaje = "OK";
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                ChatDataBase.InsertMessageInChatRoom(ref oDataBase, message);
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            Respuesta respuesta = new Respuesta
            {
                Codigo = codigo,
                Mensaje = mensaje,
            };
            return respuesta;
        }

        #endregion

        #region Usuarios

        public static RespuestaUserInfo Login(string Usuario, string Password)
        {
            int codigo = 0;
            string mensaje = "OK";
            UserInformation user = new UserInformation();
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                user = ChatDataBase.Login(ref oDataBase, Usuario, Password);
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            RespuestaUserInfo respuesta = new RespuestaUserInfo
            {
                Codigo = codigo,
                Mensaje = mensaje,
                Data = user
            };
            return respuesta;
        }

        public static Respuesta RegisterUsuario(RegisterViewModel userVM)
        {
            int codigo = 0;
            string mensaje = "OK";
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                string NewPassword = userVM.Password;
                UserInformation user = new UserInformation(userVM);
                ChatDataBase.RegisterUsuario(ref oDataBase, user, NewPassword);
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            Respuesta respuesta = new Respuesta
            {
                Codigo = codigo,
                Mensaje = mensaje,
            };
            return respuesta;
        }

        #region No usados en este proyecto

        public static RespuestaUsuarios GetUsuarios()
        {
            int codigo = 0;
            string mensaje = "OK";
            List<UserInformation> users = new List<UserInformation>();
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                DataTable dt = ChatDataBase.SelectUsuarios(ref oDataBase);
                foreach (DataRow row in dt.Rows)
                {
                    UserInformation user = new UserInformation(row);
                    users.Add(user);
                }
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            RespuestaUsuarios respuesta = new RespuestaUsuarios
            {
                Codigo = codigo,
                Mensaje = mensaje,
                Data = users
            };
            return respuesta;
        }

        public static RespuestaUserInfo GetUsuario(string Usuario)
        {
            int codigo = 0;
            string mensaje = "OK";
            UserInformation user = new UserInformation();
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                DataTable dt = ChatDataBase.SelectUsuario(ref oDataBase, Usuario);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    user = new UserInformation(row);
                }
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            RespuestaUserInfo respuesta = new RespuestaUserInfo
            {
                Codigo = codigo,
                Mensaje = mensaje,
                Data = user
            };
            return respuesta;
        }

        public static RespuestaUserInfo GetUsuario(int Id)
        {
            int codigo = 0;
            string mensaje = "OK";
            UserInformation user = new UserInformation();
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                DataTable dt = ChatDataBase.SelectUsuario(ref oDataBase, Id);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    user = new UserInformation(row);
                }
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            RespuestaUserInfo respuesta = new RespuestaUserInfo
            {
                Codigo = codigo,
                Mensaje = mensaje,
                Data = user
            };
            return respuesta;
        }        

        public static Respuesta ResetPassword(ResetPasswordViewModelApp userVM)
        {
            int codigo = 0;
            string mensaje = "OK";
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                string userAdmin = userVM.userAdmin;
                string Usuario = userVM.Usuario;
                string NewPassword = userVM.NuevaClave;
                UserInformation user = new UserInformation();
                user.Usuario = Usuario;
                ChatDataBase.ResetPassword(ref oDataBase, Usuario, NewPassword);
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            Respuesta respuesta = new Respuesta
            {
                Codigo = codigo,
                Mensaje = mensaje,
            };
            return respuesta;
        }

        public static Respuesta ChangePassword(SetPasswordViewModelApp userVM)
        {
            int codigo = 0;
            string mensaje = "OK";
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                string Usuario = userVM.Usuario;
                string Password = userVM.Clave;
                string NewPassword = userVM.NuevaClave;
                UserInformation user = new UserInformation();
                user.Usuario = Usuario;
                bool EsPwd = ChatDataBase.IsCurrentPassword(ref oDataBase, Usuario, Password);
                if (EsPwd)
                {
                    ChatDataBase.ChangePassword(ref oDataBase, Usuario, Password, NewPassword);
                }
                else
                {
                    codigo = 9;
                    mensaje = "Clave actual no es la correcta";
                    mensaje = String.Format("Error. Detalles: {0}", mensaje);
                }
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            Respuesta respuesta = new Respuesta
            {
                Codigo = codigo,
                Mensaje = mensaje,
            };
            return respuesta;
        }        

        public static Respuesta UpdateUsuario(UsuarioViewModel userVM)
        {
            int codigo = 0;
            string mensaje = "OK";
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                UserInformation user = new UserInformation(userVM);
                ChatDataBase.UpdateUsuario(ref oDataBase, user);
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            Respuesta respuesta = new Respuesta
            {
                Codigo = codigo,
                Mensaje = mensaje,
            };
            return respuesta;
        }

        public static Respuesta DeleteUsuario(int Id)
        {
            int codigo = 0;
            string mensaje = "OK";
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                ChatDataBase.DeleteUsuario(ref oDataBase, Id);
            }
            catch (Exception exc)
            {
                codigo = 9;
                mensaje = String.Format("Error. Detalles: {0}", exc.Message);
            }
            oDataBase.Close();
            Respuesta respuesta = new Respuesta
            {
                Codigo = codigo,
                Mensaje = mensaje,
            };
            return respuesta;
        }

        #endregion

        #endregion

        #endregion
    }
}