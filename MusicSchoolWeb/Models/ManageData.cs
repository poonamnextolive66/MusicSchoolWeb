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
                string query = "select * FROM LessionMaster_tbl";
                dtContainer = db.DataTable(query);
                if (dtContainer.Rows.Count > 0)
                {
                    foreach (DataRow rdr in dtContainer.Rows)
                    {
                        Lesson topic = new Lesson();
                        topic.LessonId = Convert.ToInt32(rdr["Id"]);
                        topic.LessonName = rdr["LessionName"].ToString();
                        topic.status = 1;
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
        public string deletetopic(string id)
        {
            string status = "false";
            string Query = "Delete From Topics_tbl Where Id = " + id;
            status = db.Delete(Query);
            return status;
        }
        public string deletelession(string id)
        {
            string status = "false";
            string Query = "Delete From LessionMaster_tbl Where Id = " + id;
            status = db.Delete(Query);
            return status;
        }
        public string deleteaudio(string id)
        {
            string status = "false";
            string Query = "Delete From Audio_tbl Where Id = " + id;
            status = db.Delete(Query);
            return status;
        }
        public List<Topic> GetTopicbylasson(string lasson)
        {
            List<Topic> retval = new List<Topic>();
            dtContainer = new DataTable();
            try
            {
                string query = "select * FROM Topics_tbl where LessionId='" + lasson + "' ";
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
        public List<Lesson> GetAudioFiles()
        {
            List<Lesson> audiolist = new List<Lesson>();
            string query = "SELECT LessionMaster_tbl.LessionName, Topics_tbl.TopicName, Audio_tbl.Audiofiles,Audio_tbl.Id From LessionMaster_tbl JOIN Audio_tbl ON Audio_tbl.LessionId = LessionMaster_tbl.Id inner JOIN Topics_tbl ON Audio_tbl.TopicId = Topics_tbl.Id;";
            audiolist = db.GetData(query);
            return audiolist;
        }
        public string InsertAudioFiles(Lesson lesson)
        {
            string msg = "false";
            string selectquery = "select * from Audio_tbl where LessionId='" + lesson.LessonId + "'and TopicId='"+lesson.TopicId+ "'";
            DataTable dt = db.DataTable(selectquery);
            if (dt.Rows.Count == 0)
            {
                string Query = "Insert into Audio_tbl values ('" + lesson.LessonId + "','" + lesson.TopicId + "','" + lesson.AudioFilename + "')";
                msg = db.InsertUpdateDelete(Query);
            }
            else
            {
                msg = "exist";
            }
            return msg;
        }

        public string InsertLesson(Lesson lesson)
        {
            string msg = "false";
            string selectquery = "select * from LessionMaster_tbl where LessionName='" + lesson.LessonName + "'";
            DataTable dt = db.DataTable(selectquery);
            if(dt.Rows.Count==0)
            {
                string Query = "Insert into LessionMaster_tbl values ('" + lesson.LessonName + "')";
                msg = db.InsertUpdateDelete(Query);
            }
            else
            {
                msg = "exist";
            }
            return msg;
        }
        public string InsertTopic(Lesson lesson)
        {
            string msg = "false";
            string selectquery = "select * from Topics_tbl where LessionId='" + lesson.LessonId + "' and TopicName='"+lesson.TopicName+"'";
            DataTable dt = db.DataTable(selectquery);
            if (dt.Rows.Count == 0)
            {
                string Query = "Insert into Topics_tbl values ('" + lesson.LessonId + "','" + lesson.TopicName + "')";
                msg = db.InsertUpdateDelete(Query);
            }
            else
            {
                msg = "exist";
            }
            return msg;
        }
    }
}