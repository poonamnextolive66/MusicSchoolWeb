using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicSchoolWeb.Models;


namespace MusicSchoolWeb.Controllers
{
    public class UserController : Controller
    {
        List<AudioFile> audiolist = new List<AudioFile>();
        ManageData manage = new ManageData();
        string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Play_Match_Pitch()
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetAllAudioFile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    AudioFile audio = new AudioFile();
                    audio.ID = Convert.ToInt32(rdr["ID"]);
                    audio.Name = rdr["Name"].ToString();
                    audio.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    audio.FilePath = rdr["FilePath"].ToString();
                    audiolist.Add(audio);
                }
            }
            return View(audiolist); //
        }
        public ActionResult PlayPiano()
        {
            return View();
        }
        public ActionResult Piano(string lessonId,string topicId)
        {
            List<Lesson> dt = manage.GetAudioSample(lessonId, topicId);
            //FileInfo[] obj = ReadFiles();
            ViewBag.song = dt;
            return View();
        }
        private FileInfo[] ReadFiles()
        {
            FileInfo[] files = null;
            string path1 = Path.Combine(Server.MapPath(@"/AudioFiles/"));
            if (System.IO.Directory.Exists(path1))
            {
                DirectoryInfo directory = new DirectoryInfo(path1);
                files = directory.GetFiles();
                List<byte[]> audiobytelst = new List<byte[]>();
                byte[] audiobyte;
                foreach (var item in files)
                {
                    audiobyte = System.IO.File.ReadAllBytes(item.FullName);
                    audiobytelst.Add(audiobyte);
                }
            }
            return files;
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            string path2 = Server.MapPath(@"/UploadAudio/");
            HttpFileCollectionBase files = Request.Files;
            byte[] audiobyte2;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                file.SaveAs(path2 + file.FileName);
                string path1 = path2 + file.FileName;
                audiobyte2 = System.IO.File.ReadAllBytes(path1);
            }
            FileInfo[] fils = this.ReadFiles();
            return Json(files.Count + " Files Uploaded!");
        }
    }
}