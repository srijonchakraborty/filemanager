using System;
using System.Collections.Generic;

#nullable disable

namespace FileManagerAPI.Common.Models
{
    public partial class Upload
    {
        public Upload()
        {
            UploadItems = new HashSet<UploadItem>();
        }

        public long UploadId { get; set; }
        public string UploadCode { get; set; }
        public string UploadDescription { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual ICollection<UploadItem> UploadItems { get; set; }
    }
}
