using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MusicSchoolWeb.Models
{
    public class SqlData
    {
        string con = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        DataTable dtContainer;
        public DataTable DataTable(string query)
        {
            DataTable dtbl = new DataTable();
            SqlConnection sqlCon = new SqlConnection(con);
            SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
            sqlDa.Fill(dtbl);
            return dtbl;
        }
    
}
}