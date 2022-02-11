using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Common
{
    #region Enums

   
    public enum LoginStatusEnum
    {
        Success = 0,
        Error = 1,
        Unsuccesful = 2,
        OTPExpire = 3
    }
    public enum FileUploadInfoStatusEnum
    {
        Success = 0,
        Error = 1,
        Unsuccesful = 2,
        NoFileToUpload = 3
    }
    public enum StatusEnum : short
    {
        Active = 1,
        Discontinued = 2
    }

    #endregion   

}
