using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSchoolWeb.Models
{
    public class Lesson
    {
       public int Id { get; set; }
        public string LessonName { get; set; }
        public int LessonId { get; set; }
        public string TopicName { get; set; }
    }
}