using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class ResponseModel
    {
        public int Code { get; set; }
        public Object Data { get; set; }
        public bool Success { get; set; }
    }
}