using FileManagerAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Common.Requests
{
    public class UploadRequest : Upload
    {
        
    }
    public class UploadSearchRequest 
    {
        public string UploadCode { get; set; }
        public string UploadDescription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedFromDate { get; set; }
        public DateTime? CreatedToDate { get; set; }
    }
    
}
