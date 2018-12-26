using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocSignCallingAPI.Models
{
    public class DocuSignRequestData
    {
        public string recipients { get; set; }
        public string fileData { get; set; }
    }
}