using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.Models
{
    public class UserInformation
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Nombres { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Correo { get; set; }
        public bool Autenticado { get; set; }

        public UserInformation(DataRow RegUsr)
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
                this.Usuario = RegUsr["Usuario"].ToString();
            if (columnas.Contains("Nombre"))
                this.Nombres = RegUsr["Nombre"].ToString();

            if (columnas.Contains("Codigo"))
                this.Codigo = RegUsr["Codigo"].ToString();
            if (columnas.Contains("Descripcion"))
                this.Descripcion = RegUsr["Descripcion"].ToString();
            if (columnas.Contains("Correo"))
                this.Correo = RegUsr["Correo"].ToString();
        }

        
        public UserInformation(RegisterViewModel userVM)
        {
            this.Id = userVM.Id;
            this.Usuario = userVM.Usuario;
            this.Nombres = userVM.Nombre;
            this.Codigo = userVM.Usuario;
            this.Descripcion = userVM.Nombre;
            this.Correo = userVM.Email;
        }

        public UserInformation(UsuarioViewModel userVM)
        {
            this.Id = userVM.Id;
            this.Usuario = userVM.Usuario;
            this.Nombres = userVM.Nombre;
            this.Codigo = userVM.Codigo;
            this.Descripcion = userVM.Descripcion;
            this.Correo = userVM.Correo;
        }

        public UserInformation()
        {
            this.Id = 0;
            this.Usuario = "";
            this.Nombres = "";
            this.Codigo = "";
            this.Descripcion = "";
            this.Correo = "";
        }
    }
}