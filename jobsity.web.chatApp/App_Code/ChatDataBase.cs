using jobsity.web.chatApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.App_Code
{
    public class ChatDataBase
    {
        #region Usuarios

        public static UserInformation Login(ref MySQLData oDataBase, string Usuario, string Password)
        {
            UserInformation user = new UserInformation();
            string cipherpassword = CryptoBase.Encrypt(Password);
            string SQLQuery = "SELECT Id, Usuario, Nombre, Codigo, Descripcion, Activo, Correo ";
            SQLQuery += "FROM Usuarios ";
            SQLQuery += "WHERE Usuario=?Usuario ";
            SQLQuery += "AND Clave=?Clave";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Usuario", Usuario);
            oDataBase.AddParameter("Clave", cipherpassword);
            DataTable tblUsuario = oDataBase.Select(SQLQuery);
            int NumRegistros = tblUsuario.Rows.Count;
            bool Autenticado = NumRegistros == 1;
            if (Autenticado)
            {
                DataRow RegUsr = tblUsuario.Rows[0];
                user = new UserInformation(RegUsr);
            }
            user.Autenticado = Autenticado;
            return user;
        }

        public static int RegisterUsuario(ref MySQLData oDataBase, UserInformation user, string NewPassword)
        {
            string Usuario = user.Usuario;
            string Nombre = user.Nombres;
            string Codigo = user.Codigo;
            string Descripcion = user.Descripcion;
            string Correo = user.Correo;
            string ciphernewpassword = CryptoBase.Encrypt(NewPassword);
            string SQLNonQuery = "INSERT INTO Usuarios(Usuario,Clave,Nombre,Codigo,Descripcion,Correo) VALUES";
            SQLNonQuery += "(?Usuario,?Clave,?Nombre,?Codigo,?Descripcion,?Correo)";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Usuario", Usuario);
            oDataBase.AddParameter("Clave", ciphernewpassword);
            oDataBase.AddParameter("Nombre", Nombre);
            oDataBase.AddParameter("Codigo", Codigo);
            oDataBase.AddParameter("Descripcion", Descripcion);
            oDataBase.AddParameter("Correo", Correo);
            return oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        #region No usados en este proyecto

        public static DataTable SelectUsuarios(ref MySQLData oDataBase)
        {
            string SQLQuery = "SELECT Id, Usuario, Nombre, Codigo, Descripcion, Correo ";
            SQLQuery += "FROM Usuarios ";
            oDataBase.ClearParameters();
            return oDataBase.Select(SQLQuery);
        }

        public static DataTable SelectUsuario(ref MySQLData oDataBase, string Usuario)
        {
            string SQLQuery = "SELECT Id, Usuario, Nombre, Codigo, Descripcion, Correo ";
            SQLQuery += "FROM Usuarios ";
            SQLQuery += "WHERE Usuario=?Usuario ";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Usuario", Usuario);
            return oDataBase.Select(SQLQuery);
        }

        public static DataTable SelectUsuario(ref MySQLData oDataBase, int Id)
        {
            string SQLQuery = "SELECT Id, Usuario, Nombre, Codigo, Descripcion, Correo ";
            SQLQuery += "FROM Usuarios ";
            SQLQuery += "WHERE Id=?Id ";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Id", Id);
            return oDataBase.Select(SQLQuery);
        }        

        public static int ResetPassword(ref MySQLData oDataBase, string Usuario, string NewPassword)
        {
            string ciphernewpassword = CryptoBase.Encrypt(NewPassword);
            string SQLNonQuery = "UPDATE Usuarios ";
            SQLNonQuery += "SET Clave=?NuevaClave ";
            SQLNonQuery += "WHERE Usuario=?Usuario ";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Usuario", Usuario);
            oDataBase.AddParameter("NuevaClave", ciphernewpassword);
            return oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        public static int ChangePassword(ref MySQLData oDataBase, string Usuario, string Password, string NewPassword)
        {
            string cipherpassword = CryptoBase.Encrypt(Password);
            string ciphernewpassword = CryptoBase.Encrypt(NewPassword);
            string SQLNonQuery = "UPDATE Usuarios ";
            SQLNonQuery += "SET Clave=?NuevaClave ";
            SQLNonQuery += "WHERE Usuario=?Usuario ";
            SQLNonQuery += "AND Clave=?Clave";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Usuario", Usuario);
            oDataBase.AddParameter("Clave", cipherpassword);
            oDataBase.AddParameter("NuevaClave", ciphernewpassword);
            return oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        public static bool IsCurrentPassword(ref MySQLData oDataBase, string Usuario, string Password)
        {
            string cipherpassword = CryptoBase.Encrypt(Password);
            string SQLQuery = "SELECT COUNT(*) ";
            SQLQuery += "FROM Usuarios ";
            SQLQuery += "WHERE Usuario=?Usuario ";
            SQLQuery += "AND Clave=?Clave";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Usuario", Usuario);
            oDataBase.AddParameter("Clave", cipherpassword);
            string count = oDataBase.ExecuteScalar(SQLQuery).ToString();
            int NumRegistros = 0;
            int.TryParse(count, out NumRegistros);
            return NumRegistros == 1;
        }        

        public static int UpdateUsuario(ref MySQLData oDataBase, UserInformation user)
        {
            int Id = user.Id;
            string Usuario = user.Usuario;
            string Nombre = user.Nombres;
            string Codigo = user.Codigo;
            string Descripcion = user.Descripcion;
            string Correo = user.Correo;
            string SQLNonQuery = "UPDATE Usuarios ";
            SQLNonQuery += "SET Usuario=?Usuario, Nombre=?Nombre, ";
            SQLNonQuery += "Codigo=?Codigo, Descripcion=?Descripcion, Correo=?Correo ";
            SQLNonQuery += "WHERE Id=?Id";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Id", Id);
            oDataBase.AddParameter("Usuario", Usuario);
            oDataBase.AddParameter("Nombre", Nombre);
            oDataBase.AddParameter("Codigo", Codigo);
            oDataBase.AddParameter("Descripcion", Descripcion);
            oDataBase.AddParameter("Correo", Correo);
            return oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        public static int DeleteUsuario(ref MySQLData oDataBase, int Id)
        {
            string SQLNonQuery = "DELETE FROM Usuarios ";
            SQLNonQuery += "WHERE Id=?Id";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Id", Id);
            return oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        #endregion

        #endregion

        #region Chat Room

        public static int InsertMessageInChatRoom(ref MySQLData oDataBase, ChatMessage message)
        {
            string Usuario = message.Sender;
            string Message = message.Message;
            string SQLNonQuery = "INSERT INTO chatroom(Usuario, Message, SentDate) VALUES";
            SQLNonQuery += "(?Usuario,?Message,now())";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Usuario", Usuario);
            oDataBase.AddParameter("Message", Message);
            int n = oDataBase.ExecuteNonQuery(SQLNonQuery);
            int Id = SelectLastInsertId(ref oDataBase);
            message.Id = Id;
            return n;
        }

        #endregion

        #region Read Messages

        public static DataTable SelectNotReadMessages(ref MySQLData oDataBase, string Usuario)
        {
            string TopMessagesInChatRoom = ConfigurationManager.AppSettings["TopMessagesInChatRoom"];
            int Top = 0;
            bool ParseOK = int.TryParse(TopMessagesInChatRoom, out Top);
            if (!ParseOK) { Top = 50; }
            string SQLQuery = "SELECT Id, Usuario, Message, SentDate ";
            SQLQuery += "FROM chatroom c ";
            SQLQuery += "WHERE Usuario != ?Usuario ";
            SQLQuery += "AND (SELECT COUNT(*) ";
            SQLQuery += "FROM readmessages r ";
            SQLQuery += "WHERE r.Id = c.Id ";
            SQLQuery += "AND r.Usuario = ?Usuario ";
            SQLQuery += ") = 0 ";
            SQLQuery += "ORDER BY SentDate ";
            SQLQuery += "LIMIT ?Top ";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Usuario", Usuario);
            oDataBase.AddParameter("Top", Top);
            return oDataBase.Select(SQLQuery);
        }

        public static int InsertReadMessage(ref MySQLData oDataBase, ChatMessage message)
        {
            int Id = message.Id;
            string Usuario = message.Receiver;
            string SQLNonQuery = "INSERT INTO readmessages(Id,Usuario,ReadDate) VALUES";
            SQLNonQuery += "(?Id,?Usuario,now())";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Id", Id);
            oDataBase.AddParameter("Usuario", Usuario);            
            int n = oDataBase.ExecuteNonQuery(SQLNonQuery);
            return n;
        }

        #endregion

        public static int SelectLastInsertId(ref MySQLData oDataBase)
        {
            string SQLQuery = "SELECT LAST_INSERT_ID() ";
            oDataBase.ClearParameters();
            object objcount = oDataBase.ExecuteScalar(SQLQuery);
            int count = 0;
            int.TryParse(objcount.ToString(), out count);
            return count;
        }
    }
}