
using FileManager.Common.Requests;
using FileManager.Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Services.Contracts.Files
{
    public interface IFileUploadService
    {
        Task<UploadResponse> GetFilesByIDAsync(long uploadId);
        Task<UploadResponse> SaveUploadFilesAsync(UploadRequest item);
        Task<UploadListResponse> GetFilesAsync(UploadSearchRequest itemSearchRequest);
    }
}

