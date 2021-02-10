using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class ResultViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool isSelected { get; set; }

    }
}