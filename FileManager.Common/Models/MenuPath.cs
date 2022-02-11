using System;
using System.Collections.Generic;

#nullable disable

namespace FileManagerAPI.Common.Models
{
    public partial class MenuPath
    {
        public long MenuPathsId { get; set; }
        public long? UserId { get; set; }
        public string MenuPath1 { get; set; }
        public string MenuPathTitle { get; set; }
        public bool? IsActive { get; set; }
    }
}
