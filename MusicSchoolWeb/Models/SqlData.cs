using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace MusicSchoolWeb.Models
{
    public class SqlData
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString.ToString());
        DataTable dtContainer;
        //public void openConnection()
        //{
        //    try
        //    {
        //        sqlConnection.Close();
        //        sqlConnection = new SqlConnection(con);
        //        sqlConnection.Open();
               
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        public DataTable DataTable(string query)
        {
            DataTable dtbl = new DataTable();
            con.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter(query, con);
            con.Close();
            sqlDa.Fill(dtbl);
            return dtbl;
        }
        public bool Delete(string sql)
        {
            bool status = false;
            try { 
          
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                status = true;
                    }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return status;
        }
       
    }
}