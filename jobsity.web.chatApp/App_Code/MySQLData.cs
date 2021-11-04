using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace jobsity.web.chatApp.App_Code
{
    public class MySQLData
    {
        private MySqlConnection oCon;
        private MySqlCommand oCom;
        private MySqlTransaction oTrn;

        public MySQLData()
        {
            string MySQLConn = ConfigurationManager.ConnectionStrings["MySQLConn"].ConnectionString;
            this.oCon = new MySqlConnection(MySQLConn);
            this.oCom = this.oCon.CreateCommand();
        }

        public void Open()
        {
            this.oCon.Open();
        }

        public void Close()
        {
            if (this.oCon.State == ConnectionState.Open)
                this.oCon.Close();
        }

        public void BeginTransaction()
        {
            this.oTrn = this.oCon.BeginTransaction();
            this.oCom.Transaction = this.oTrn;
        }

        public void CommitTransaction()
        {
            this.oTrn.Commit();
        }

        public void RollbackTransaction()
        {
            if (this.oTrn != null)
                this.oTrn.Rollback();
        }

        public DataTable Select(string SQLQuery)
        {
            return this.SelectFrom(SQLQuery, CommandType.Text);
        }

        public DataTable SelectFromStoredProcedure(string StoredProcedure)
        {
            return this.SelectFrom(StoredProcedure, CommandType.StoredProcedure);
        }

        public DataSet SelectDataSetFromStoredProcedure(string StoredProcedure)
        {
            return this.SelectDataSetFrom(StoredProcedure, CommandType.StoredProcedure);
        }

        private DataTable SelectFrom(string StoredProcedure, CommandType tipo)
        {
            this.oCom.CommandText = StoredProcedure;
            this.oCom.CommandType = tipo;
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(this.oCom);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        private DataSet SelectDataSetFrom(string StoredProcedure, CommandType tipo)
        {
            this.oCom.CommandText = StoredProcedure;
            this.oCom.CommandType = tipo;
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(this.oCom);
            DataSet ds = new DataSet();
            mySqlDataAdapter.Fill(ds);
            return ds;
        }

        public object ExecuteScalar(string SQL)
        {
            this.oCom.CommandText = SQL;
            this.oCom.CommandType = CommandType.Text;
            return this.oCom.ExecuteScalar();
        }

        public int ExecuteNonQuery(string SQLNonQuery)
        {
            return this.ExecuteSQLNonQuery(SQLNonQuery, CommandType.Text);
        }

        public int ExecuteNonQueryFromStoredProcedure(string StoredProcedure)
        {
            return this.ExecuteSQLNonQuery(StoredProcedure, CommandType.StoredProcedure);
        }

        private int ExecuteSQLNonQuery(string StoredProcedure, CommandType Tipo)
        {
            this.oCom.CommandText = StoredProcedure;
            this.oCom.CommandType = Tipo;
            return this.oCom.ExecuteNonQuery();
        }

        public void ClearParameters()
        {
            this.oCom.Parameters.Clear();
        }

        public void AddParameter(string Name, object Value)
        {
            this.oCom.Parameters.AddWithValue(Name, Value);
        }

        public void AddParameterNull(string name)
        {
            this.oCom.Parameters.AddWithValue(name, DBNull.Value);
        }
    }
}