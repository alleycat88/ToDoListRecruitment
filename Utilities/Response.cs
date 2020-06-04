using System;
using System.Collections.Generic;

namespace ToDoListRecruitment.Utility
{
    public class Response
    {
        public bool success{ get; set;}
        public int status { get; set; }
        public string message { get; set; }
        public string url { get; set; }
        public Object data {get;set;}
        public int totalData {get;set;}
    }
}