using System;
using System.Collections.Generic;

#nullable disable

namespace FileManagerAPI.Common.Models
{
    public partial class UploadItem
    {
        public long UploadItemId { get; set; }
        public long? UploadId { get; set; }
        public string FileOrginalName { get; set; }
        public string FileGeneratedName { get; set; }
        public string FileType { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Upload Upload { get; set; }
    }
}
