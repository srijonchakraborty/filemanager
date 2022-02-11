using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using FileManager.Services.Contracts.Files;
using FileManagerAPI.Common;
using Microsoft.Extensions.Options;
using FileManager.Common.Responses;
using FileManager.Services.Models;
using FileManagerAPI.Common.Models;
using FileManager.Common.Requests;
using Microsoft.EntityFrameworkCore;
using FileManager.Common;

namespace FileManager.Services.Services.Files
{
    public class FileUploadService : IFileUploadService
    {
        private readonly AppSettings _settings;
        public FileUploadService(IOptions<AppSettings> settings)
        {
            _settings = settings?.Value;
        }
        public async Task<UploadResponse> GetFilesByIDAsync(long uploadId) 
        {
            UploadResponse response = null;
            try
            {
                FileManagerContext context = new FileManagerContext(_settings);
               
                Upload itm = await (context.Uploads.Where(s => s.UploadId == uploadId).FirstOrDefaultAsync());
                if (itm != null)
                {
                    response = (UploadResponse)itm;
                }
                if (response.UploadId > 0)
                {
                    response.UploadItems = await context.UploadItems.Where(s => s.UploadId == uploadId).ToListAsync();
                }
                response.Status = FileUploadInfoStatusEnum.Success;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return response;
        }
        public async Task<UploadResponse> SaveUploadFilesAsync(UploadRequest item) {
            UploadResponse response = null;
            try
            {
                FileManagerContext context = new FileManagerContext(_settings);
                await context.Uploads.AddAsync(item);
                await context.SaveChangesAsync();
                response = new UploadResponse();
                response.Status = FileUploadInfoStatusEnum.Success;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return response;
        }
        public async Task<UploadListResponse> GetFilesAsync(UploadSearchRequest itemSearchRequest) {

            UploadListResponse response = null;
            try
            {
                FileManagerContext context = new FileManagerContext(_settings);
               


                response.Status = FileUploadInfoStatusEnum.Success;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return response;

        }
    }
}
