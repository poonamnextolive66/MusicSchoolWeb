using MusicSchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Collections;



namespace MusicSchoolWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        List<Lesson> lessons = new List<Lesson>();
        string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        ManageData manage = new ManageData();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Music_Course()
        {
            return View();
        }
        public ActionResult Sub_Category()
        {
            return View();
        }

        public ActionResult UploadAudio()
        {
            return View();
        }
        public ActionResult Lesson()
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetAllLession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Lesson lesion = new Lesson();
                    lesion.Id = Convert.ToInt32(rdr["Id"]);
                    lesion.LessonName = rdr["LessionName"].ToString();
                    lessons.Add(lesion);
                }
            }
            return View(lessons); //
        }
        public ActionResult AddLesson(Lesson lesson)
        {
            string Query = "Insert into LessionMaster_tbl values ('" + lesson.LessonName + "')";
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand(Query, con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.RecordsAffected==1)
                {
                    TempData["msgsuccess"] = "Yes";
                    return RedirectToAction("Lesson", "Admin");
                }
                else
                {

                }
            }
            return View();
        }
        public ActionResult Topics()
        {
            List<Lesson> less = new List<Lesson>();
            less = manage.GetLesson();
            ViewBag.lesson = less;
            lessons = manage.GetAllTopics();
            return View(lessons); //
        }
        public ActionResult AddTopic(Lesson lesson)
        {
            string Query = "Insert into Topics_tbl values ('" + lesson.LessonId + "','" + lesson.TopicName + "')";
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand(Query, con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.RecordsAffected == 1)
                {
                    TempData["msgsuccess"] = "Yes";
                    return RedirectToAction("Topics", "Admin");
                }
            }
            return View();
        }



        }
    }