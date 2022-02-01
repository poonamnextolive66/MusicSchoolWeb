using System;
using System.Collections;
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
        public List<Lesson> GetData(string query)
        {
            List<Lesson> audiolist = new List<Lesson>();
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Lesson audio = new Lesson();
                audio.Id = Convert.ToInt32(rdr["ID"]);
                audio.LessonName = rdr["LessionName"].ToString();
                audio.TopicName = rdr["TopicName"].ToString();
                audio.AudioFilename = rdr["Audiofiles"].ToString();
                audiolist.Add(audio);
            }
            return audiolist;
        }
        public bool InsertUpdateDelete(string query)
        {
            bool msg = false;
            SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.RecordsAffected == 1)
                {
                msg = true;
                }
            return msg;
        }
    }
}