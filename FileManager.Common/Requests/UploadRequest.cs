using FileManagerAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Common.Responses
{
    public class UploadResponse : Upload, IResponseBase
    {
        public UploadResponse()
        {
            ReturnMessage = new List<string>();
        }
        public int ReturnStatus { get; set; }
        public List<string> ReturnMessage { get; set; }
        public FileUploadInfoStatusEnum Status { get; set; }
        
    }

    public class UploadListResponse : Upload, IResponseBase
    {
        public UploadListResponse()
        {
            ReturnMessage = new List<string>();
            Uploads = new List<Upload>();
        }
        public List<Upload> Uploads { get; set; }
        public int ReturnStatus { get; set; }
        public List<string> ReturnMessage { get; set; }
        public FileUploadInfoStatusEnum Status { get; set; }

    }
    
}
