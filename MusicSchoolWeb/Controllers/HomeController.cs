using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Security.Cryptography;
using System.Text;
using MusicSchoolWeb.Models;
using SoundFingerprinting;
using SoundFingerprinting.Tests;
using System.Reflection;

namespace MusicSchoolWeb.Controllers
{
    public class HomeController : Controller
    {
        List<AudioFile> audiolist = new List<AudioFile>();
        string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        ManageData manage = new ManageData();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult subscription()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UploadAudio()
        {
            List<Lesson> audiolist = new List<Lesson>();
            audiolist = manage.GetAudioFiles();
            return View(audiolist); //
        }
        //[HttpPost]
        //public ActionResult UploadAudio(HttpPostedFileBase fileupload)
        //{
        //    if (fileupload != null)
        //    {
        //        string fileName = Path.GetFileName(fileupload.FileName);
        //        int fileSize = fileupload.ContentLength;
        //        int Size = fileSize / 1000000;

        //        var Hash_Value_Of_First_File = string.Empty;
        //        var Hash_Value_Of_All_Files_One_By_One = string.Empty;
        //        string OldFiles = string.Empty;
        //        int counter = 0;

        //        fileupload.SaveAs(Server.MapPath("~/RawFiles/" + fileName));

        //        // Code for Extracting Hash Value of Currently Uploaded File Starts 
        //        using (var stream = new BufferedStream(System.IO.File.OpenRead(Server.MapPath("~/RawFiles/" + fileName)), 1200000))
        //        {
        //            SHA256Managed sha = new SHA256Managed();
        //            byte[] checksum = sha.ComputeHash(stream);
        //            Hash_Value_Of_First_File = BitConverter.ToString(checksum).Replace("-", string.Empty);
        //        }
        //        // Code for Extracting Hash Value of Currently Uploaded File Ends


        //        // Code for Extracting Hash Value of all files present in the folder to match with the new one starts
        //        string[] filePaths = Directory.GetFiles(Server.MapPath("~/AudioFiles/"));
        //        foreach (string filePath in filePaths)
        //        {
        //            OldFiles = Path.GetFileName(filePath);
        //            using (var stream = new BufferedStream(System.IO.File.OpenRead(Server.MapPath("~/AudioFiles/" + OldFiles)), 1200000))
        //            {
        //                SHA256Managed sha = new SHA256Managed();
        //                byte[] checksum = sha.ComputeHash(stream);
        //                Hash_Value_Of_All_Files_One_By_One = BitConverter.ToString(checksum).Replace("-", string.Empty);
        //            }

        //            if (Hash_Value_Of_First_File == Hash_Value_Of_All_Files_One_By_One)
        //                counter = 1;
        //        }// Code for Extracting Hash Value of all files present in the folder to match with the new one Ends

        //        if (counter == 0)
        //        {
        //            fileupload.SaveAs(Server.MapPath("~/AudioFiles/" + fileName));
        //            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        //            using (SqlConnection con = new SqlConnection(CS))
        //            {
        //                string path = "~/AudioFiles/" + fileName;
        //                string query = "insert into AudioFiles values('" + fileName + "','" + fileSize + "','" + path + "','Lesson - 1')";
        //                SqlCommand cmd = new SqlCommand(query, con);
        //                //SqlCommand cmd = new SqlCommand("spAddNewAudioFile", con);
        //                //cmd.CommandType = CommandType.StoredProcedure;
        //                con.Open();
        //                //cmd.Parameters.AddWithValue("@Name", fileName);
        //                //cmd.Parameters.AddWithValue("@FileSize", Size);
        //                //cmd.Parameters.AddWithValue("@FilePath", "~/AudioFiles/" + fileName);
        //                cmd.ExecuteNonQuery();
        //                TempData["msgsuccess"] = "Yes";
        //            }
        //        }
        //        TempData["msg"] = counter;
        //    }
        //    return RedirectToAction("UploadAudio");
        //}
        [HttpGet]
        private List<FileAttrib> GetFiles(DirectoryInfo dinfo)
        {

            List<FileAttrib> files = new List<FileAttrib>();
            foreach (var directory in dinfo.GetDirectories())
            {
                files.AddRange(this.GetFiles(directory));
            }
            return files; //
        }
        public ActionResult Delete(int? i, string filenames)
        {
            //var st = audiolist.Find(c => c.ID == i);
            //audiolist.Remove(st);
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spDeleteAudioFile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@ID", i);
                cmd.ExecuteNonQuery();

                string path = Server.MapPath("~/AudioFiles/" + filenames);
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                    TempData["msgdelete"] = "Yes";
                }
            }
            return RedirectToAction("UploadAudio");
        }
        [HttpPost]
        public JsonResult CompareAudio(string au1, string au2)
        {
            var msg = "The Tune is not Matched and Returned False. !";
            string FirstFile = au1;
            string SecondFle = au2;

            if (FirstFile != null && SecondFle != null)
            {
                string fileName = Path.GetFileName(FirstFile);
                //int fileSize = fileupload.ContentLength;
                //int Size = fileSize / 1000000;

                var Hash_Value_Of_First_File = string.Empty;
                var Hash_Value_Of_All_Files_One_By_One = string.Empty;
                string OldFiles = string.Empty;
                int counter = 0;

                //  fileupload.SaveAs(Server.MapPath("~/RawFiles/" + fileName));
                //SecondFle = SecondFle.Replace(".mp3", "");
                //SecondFle = "Lesson 1-3.mp3";
                // Code for Extracting Hash Value of Currently Uploaded File Starts 
                using (var stream = new BufferedStream(System.IO.File.OpenRead(Server.MapPath("~/AudioFiles/" + SecondFle)), 1200000))
                {
                    SHA256Managed sha = new SHA256Managed();
                    byte[] checksum = sha.ComputeHash(stream);
                    Hash_Value_Of_First_File = BitConverter.ToString(checksum).Replace("-", string.Empty);
                }
                // Code for Extracting Hash Value of Currently Uploaded File Ends


                // Code for Extracting Hash Value of all files present in the folder to match with the new one starts
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/AudioFiles/"));
                foreach (string filePath in filePaths)
                {
                    OldFiles = Path.GetFileName(filePath);  //FirstFile; //
                    using (var stream = new BufferedStream(System.IO.File.OpenRead(Server.MapPath("~/AudioFiles/" + OldFiles)), 1200000))
                    {
                        SHA256Managed sha = new SHA256Managed();
                        byte[] checksum = sha.ComputeHash(stream);
                        Hash_Value_Of_All_Files_One_By_One = BitConverter.ToString(checksum).Replace("-", string.Empty);
                    }

                    if (Hash_Value_Of_First_File == Hash_Value_Of_All_Files_One_By_One)
                    {

                    }
                    //msg = "The Tune is Matched and Returned True. !";
                }// Code for Extracting Hash Value of all files present in the folder to match with the new one Ends

                TempData["CompareMessage"] = counter;
            }
                return Json(new { msg  });
            }

        }
    }

