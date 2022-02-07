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
using static System.Net.WebRequestMethods;
using System.Windows;
using EO.WebBrowser.DOM;
using System.Net;
using System.Security.Policy;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Ajax.Utilities;
using ICSharpCode.AvalonEdit.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;

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
            string msg = string.Empty;
            string FirstFile = au1;
            au1 = au1.Replace("data:audio/wav;base64,", "");
            string SecondFle = au2;
            var bytes = Convert.FromBase64String(au1);
            var contents = new StreamContent(new MemoryStream(bytes));
            var hash11=GetHashSHA1(bytes);
            if (FirstFile != null && SecondFle != null)
            {
                string fileName = Path.GetFileName(FirstFile);
                var Hash_Value_Of_First_File = string.Empty;
                var Hash_Value_Of_All_Files_One_By_One = string.Empty;
                string OldFiles = string.Empty;
               int counter = 0;
                Hash_Value_Of_First_File = hash11;
               // byte[] mybyt = System.IO.File.ReadAllBytes(@"C:\Users\upkar\Downloads\testing.wav");
                byte[] mybyt = System.IO.File.ReadAllBytes(Server.MapPath("~/AudioFiles/"+au2+""));
                Hash_Value_Of_All_Files_One_By_One = GetHashSHA1(mybyt);
                if (Hash_Value_Of_First_File == Hash_Value_Of_All_Files_One_By_One)
                    {
                        counter = 1;
                    msg = "true";
                    }
                else
                {
                    counter = 0;
                    msg = "false";
                }
                    //msg = "The Tune is Matched and Returned True. !";
                // Code for Extracting Hash Value of all files present in the folder to match with the new one Ends

               // TempData["CompareMessage"] = counter;
            }
            return Json(new { msg });
        }
        private static Stream GetStreamFromUrl(string url)
        {
            byte[] musicData = null;

            using (var wc = new System.Net.WebClient())
                musicData = wc.DownloadData(url);

            return new MemoryStream(musicData);
        }

        public void GetBlobtostreamfile(string url)
        {
            url= "https://localhost:44395/24375a43-57ed-40b2-9dbe-ed22acd91dc4";
            // Creates an HttpWebRequest with the specified URL.
    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            // Sends the HttpWebRequest and waits for the response.         
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            // Pipes the stream to a higher level stream reader with the required encoding format.
            StreamReader readStream = new StreamReader(receiveStream, encode);
            Console.WriteLine("\r\nResponse stream received.");
            Char[] read = new Char[256];
            // Reads 256 characters at a time.
            int count = readStream.Read(read, 0, 256);
            Console.WriteLine("HTML...\r\n");
            while (count > 0)
            {
                // Dumps the 256 characters on a string and displays the string to the console.
                String str = new String(read, 0, count);
                Console.Write(str);
                count = readStream.Read(read, 0, 256);
            }
            Console.WriteLine("");
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
        }
        public void BlobToStringConverter(string blob)
        {
            //var audioURL = window.URL.createObjectURL(blob);
            //audio.src = audioURL;

            //var reader = new window.FileReader();
            //reader.readAsDataURL(blob);
            //reader.onloadend = function() {
            //    base64data = reader.result;
            //    console.log(base64data);
            //}
        }
        public static string GetHashSHA1(byte[] data)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                return string.Concat(sha1.ComputeHash(data).Select(x => x.ToString("X2")));
            }
        }
    }
}

