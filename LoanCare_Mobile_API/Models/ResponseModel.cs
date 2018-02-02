using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanCare_Mobile_API.Models
{
    public class ResponseModel
    {
        public Status status { get; set; }
        public Object data { get; set; }
    }

    public class Status
    {
        public int CustomErrorCode { get; set; }
        public string Message { get; set; }
    }
}