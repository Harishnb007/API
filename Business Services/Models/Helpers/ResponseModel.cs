using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.Helpers
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            status = new Status();
        }
        public ResponseModel(Object _data)
        {
            status = new Status();
            data = _data;
        }

        public ResponseModel(Object _data, int _customErrorCode, string _message)
        {
            status = new Status();
            status.CustomErrorCode = _customErrorCode;
            status.Message = _message;

            data = _data;
        }

        public Status status { get; set; }
        public Object data { get; set; }
    }

    public class Status
    {
        public int CustomErrorCode { get; set; } = 0;
        public string Message { get; set; } = "success";
    }
}
