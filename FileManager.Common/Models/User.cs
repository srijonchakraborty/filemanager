using System;
using System.Collections.Generic;

#nullable disable

namespace FileManagerAPI.Common.Models
{
    public partial class User
    {
        public User()
        {
            Uploads = new HashSet<Upload>();
        }

        public long UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserPassword { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }

        public virtual ICollection<Upload> Uploads { get; set; }
    }
}
