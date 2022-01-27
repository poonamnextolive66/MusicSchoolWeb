using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSchoolWeb.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public int LessionId { get; set; }
        public string TopicName { get; set; }
    }
}