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
using System.IO;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            List<Lesson> less = new List<Lesson>();
            List<Topic> topic = new List<Topic>();
            less = manage.GetLesson();
            ViewBag.lasson = less;
            topic = manage.GetTopictbl();
            ViewBag.topic = topic;
            return View();
        }
        public ActionResult UploadAudio()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddAudio()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAudio(Lesson lesson)
        {
            string msg = "false";
            //if (lesson.Audiofiles != null)
            //{
                string fileName = Path.GetFileName(lesson.Audiofiles.FileName);
                //int fileSize = lesson.Audiofiles.ContentLength;
                //int Size = fileSize / 1000000;

                //var Hash_Value_Of_First_File = string.Empty;
                //var Hash_Value_Of_All_Files_One_By_One = string.Empty;
                //string OldFiles = string.Empty;
                //int counter = 0;
                //lesson.Audiofiles.SaveAs(Server.MapPath("~/RawFiles/" + fileName));
                //using (var stream = new BufferedStream(System.IO.File.OpenRead(Server.MapPath("~/RawFiles/" + fileName)), 1200000))
                //{
                //    SHA256Managed sha = new SHA256Managed();
                //    byte[] checksum = sha.ComputeHash(stream);
                //    Hash_Value_Of_First_File = BitConverter.ToString(checksum).Replace("-", string.Empty);
                //}
                //string[] filePaths = Directory.GetFiles(Server.MapPath("~/AudioFiles/"));
                //foreach (string filePath in filePaths)
                //{
                //    OldFiles = Path.GetFileName(filePath);
                //    using (var stream = new BufferedStream(System.IO.File.OpenRead(Server.MapPath("~/AudioFiles/" + OldFiles)), 1200000))
                //    {
                //        SHA256Managed sha = new SHA256Managed();
                //        byte[] checksum = sha.ComputeHash(stream);
                //        Hash_Value_Of_All_Files_One_By_One = BitConverter.ToString(checksum).Replace("-", string.Empty);
                //    }
                //    if (Hash_Value_Of_First_File == Hash_Value_Of_All_Files_One_By_One)
                //        counter = 1;
                //}
                //if (counter == 0)
                //{
                    lesson.AudioFilename = lesson.Audiofiles.FileName;
                    lesson.Audiofiles.SaveAs(Server.MapPath("~/AudioFiles/" + fileName));
                    msg = manage.InsertAudioFiles(lesson);
            //        TempData["msg"] = msg;
            //    }
            //    //TempData["msg"] = counter;
            //}
            return RedirectToAction("UploadAudio","Home");
       }
        public ActionResult Lesson()
        {
            lessons = manage.GetLesson();
            return View(lessons); 
        }
        public ActionResult AddLesson(Lesson lesson)
        {
            string msg = manage.InsertLesson(lesson);
                if(msg=="true")
                {
                    TempData["msg"] = msg;
                    return RedirectToAction("Lesson", "Admin");
                }
                else
                {
                TempData["msg"] = msg;
            }
            return RedirectToAction("Lesson", "Admin");
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
            string msg = "false";
            msg = manage.InsertTopic(lesson);
            TempData["msg"] = msg;
            return RedirectToAction("Topics", "Admin");
        }
        public ActionResult DeleteTopic(string id)
        {
            string status = "false";
            status = manage.deletetopic(id);
            TempData["msg"] =status;
            return RedirectToAction("Topics", "Admin");
        }
        public ActionResult DeleteLession(string id)
        {
            string status = "false";
            status = manage.deletelession(id);
            TempData["msg"] = status;
            return RedirectToAction("Lesson", "Admin");
        }
        public JsonResult GetTopics(string lasson)
        {
            string topic = "";
            selectedValue selected = new selectedValue();
            var data = manage.GetTopicbylasson(lasson);
            if (data.Count > 0)
            {
                foreach (var d in data)
                {
                    topic += "<option value='" + d.Id + "'>" + d.TopicName + "</option>";
                }
            }
            return Json(topic, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLesson()
        {
            string topic = "";
            selectedValue selected = new selectedValue();
            var data = manage.GetLesson();
            if (data.Count > 0)
            {
                foreach (var d in data)
                {
                    topic += "<option value='" + d.LessonId + "'>" + d.LessonName + "</option>";
                }
            }

            return Json(topic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAudio(string id)
        {
            string status = "false";
            status = manage.deleteaudio(id);
              TempData["msg"] = status;
                return RedirectToAction("UploadAudio", "Home");
        }
    }
    }