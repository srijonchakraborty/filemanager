using System.Collections;
using System.Collections.Generic;

namespace FileManager.Common.Responses
{
    public interface IResponseBase
    {
         int ReturnStatus { get; set; }
         List<string> ReturnMessage { get; set; }
    }
}