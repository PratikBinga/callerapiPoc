using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocSignCallingAPI.Models
{
    public class FileUploadRequest
    {

        public string CallingApplication { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string Action { get; set; }

        public string ResponseAction { get; set; }

        public List<SupportedFileType> SupportedFile { get; set; }

        public List<ActionDataType> ActionData { get; set; }

        public List<ResponseActionDataType> ResponseActionData { get; set; }
    }

    public class ActionDataType
    {
        public long ProjectId { get; set; }
    }

    public class ResponseActionDataType
    {
        public long ProjectId { get; set; }
    }

    public class SupportedFileType
    {
        public string FileFormat { get; set; }
    }

}