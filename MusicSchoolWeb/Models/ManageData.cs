using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

namespace MusicSchoolWeb.Models
{
    public class ManageData
    {
        private readonly SqlData db = new SqlData();
        DataTable dtContainer;
        public List<Lesson> GetLesson()
        {
            List<Lesson> retval = new List<Lesson>();
            dtContainer = new DataTable();
            try
            {
                string query = "select * FROM LessionMaster_tbl ";
                dtContainer = db.DataTable(query);
                if (dtContainer.Rows.Count > 0)
                {

                    foreach (DataRow rdr in dtContainer.Rows)
                    {
                        Lesson topic = new Lesson();
                        topic.LessonId = Convert.ToInt32(rdr["Id"]);
                        topic.LessonName = rdr["LessionName"].ToString();

                        retval.Add(topic);
                    }
                }
                else
                {
                    retval = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return retval;
        }
        public List<Lesson> GetAllTopics()
        {
            List<Lesson> lessons = new List<Lesson>();
            dtContainer = new DataTable();
            try
            {
                string query = "SELECT *  from Topics_tbl INNER JOIN LessionMaster_tbl ON Topics_tbl.LessionId=LessionMaster_tbl.Id";
                dtContainer = db.DataTable(query);
                if (dtContainer.Rows.Count > 0)
                {
                    foreach (DataRow rdr in dtContainer.Rows)
                    {
                        Lesson topic = new Lesson();
                        topic.Id = Convert.ToInt32(rdr["Id"]);
                        topic.LessonId = Convert.ToInt32(rdr["LessionId"]);
                        topic.TopicName = rdr["TopicName"].ToString();
                        topic.LessonName = rdr["LessionName"].ToString();
                        lessons.Add(topic);
                    }
                    
                }
                else
                {
                    lessons = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return lessons;
        }
        //public List<Lesson> GetLessionWithTopic()
        //{
        //    List<Lesson> retval = new List<Lesson>();
        //    dtContainer = new DataTable();
        //    try
        //    {
        //        string query = "select * from LessionMaster_tbl inner join Topics_tbl on LessionMaster_tbl.Id=Topics_tbl.LessionId";
        //        dtContainer = db.DataTable(query);
        //        if (dtContainer.Rows.Count > 0)
        //        {

        //            foreach (DataRow rdr in dtContainer.Rows)
        //            {
        //                Lesson topic = new Lesson();
        //                topic.LessonId = Convert.ToInt32(rdr["Id"]);
        //                topic.LessonName = rdr["LessionName"].ToString();
        //                topic.LessonId = Convert.ToInt32(rdr["LessionId"]);
        //                topic.TopicName = rdr["TopicName"].ToString();
        //                retval.Add(topic);
        //            }
        //        }
        //        else
        //        {
        //            retval = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //    return retval;
        //}
        public List<Topic> GetTopictbl()
        {
            List<Topic> retval = new List<Topic>();
            dtContainer = new DataTable();
            try
            {
                string query = "select * FROM Topics_tbl ";
                dtContainer = db.DataTable(query);
                if (dtContainer.Rows.Count > 0)
                {
                    foreach (DataRow rdr in dtContainer.Rows)
                    {
                        Topic topic = new Topic();
                        topic.LessionId = Convert.ToInt32(rdr["LessionId"]);
                        topic.TopicName = rdr["TopicName"].ToString();
                        topic.Id = Convert.ToInt32(rdr["Id"].ToString());

                        retval.Add(topic);
                    }
                }
                else
                {
                    retval = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return retval;
        }
        public bool deletetopic(string id)
        {
            bool status = false;
            string Query = "Delete From Topics_tbl Where Id = " + id;
            status = db.Delete(Query);
            return status;
        }
        public bool deletelession(string id)
        {
            bool status = false;
            string Query = "Delete From LessionMaster_tbl Where Id = " + id;
            status = db.Delete(Query);
            return status;
        }
    }
}