using AutoMapper;
using FileManager.Services.Contracts.Files;
using FileManagerAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace FileManagerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [ValidateAntiForgeryToken]
    [Route("api/fileupload")]
    [Route("api/v{version:apiVersion}/fileupload")]
    public class FileUploadController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileUploadService _service;
        private readonly AppSettings _appSettings;
        private readonly ILogger<FileUploadController> _logger;


        public FileUploadController(IFileUploadService service, IMapper mapper, ILogger<FileUploadController> logger, IOptions<AppSettings> appSettings)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
            _appSettings = appSettings?.Value;
        }


        //[HttpPost("uploadFiles")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BooleanResponse))]
        //public async Task<IActionResult> SaveUploadFiles([FromForm] UploadFileRequest request)
        //{
        //    return null;
        //    BooleanResponse response = new BooleanResponse();

        //    if (request == null)
        //    {
        //        response.ReturnStatus = StatusCodes.Status417ExpectationFailed;
        //        response.ReturnMessage.Add("Parameter value is null.");
        //        return BadRequest(response);
        //    }
        //    if (request.IsForSubmit == true
        //        && (request.CurrentUploadedFiles == null || request.CurrentUploadedFiles.Count <= 0)
        //        && (request.PreviousFiles == null || request.PreviousFiles.Count <= 0)
        //        )
        //    {
        //        response.ReturnStatus = StatusCodes.Status417ExpectationFailed;
        //        response.ReturnMessage.Add("There is no attachment to save.");
        //        return BadRequest(response);
        //    }

        //    if (request.IsForSubmit == false
        //        && (request.CurrentUploadedFiles == null || request.CurrentUploadedFiles.Count <= 0))
        //    {
        //        response.ReturnStatus = StatusCodes.Status417ExpectationFailed;
        //        response.ReturnMessage.Add("There is no attachment to save.");
        //        return BadRequest(response);
        //    }
            
        //    //Need to uncomment
        //    //string ipAddress = HttpContext.Connection.GetIpAddress();

        //    //int oTPLoginId = Convert.ToInt32(HttpContext.User.GetClaimValue("OTPLoginID") ?? "0");
        //    //if (oTPLoginId != request.OTPLoginId)
        //    //{
        //    //    response.ReturnStatus = StatusCodes.Status403Forbidden;
        //    //    response.ReturnMessage.Add("You are not authorise to do this action (Anonymous Access).");
        //    //    return BadRequest(response);
        //    //}

        //    try
        //    {
        //        FileIOPermission fp = new FileIOPermission(FileIOPermissionAccess.AllAccess, _appSettings.UploadFolderPathSilverlightDocument);
        //        try
        //        {
        //            fp.Demand();
        //        }
        //        catch (SecurityException)
        //        {
        //        }

        //        bool exists = System.IO.Directory.Exists(_appSettings.UploadFolderPathSilverlightDocument);

        //        if (!exists)
        //            System.IO.Directory.CreateDirectory(_appSettings.UploadFolderPathSilverlightDocument);

        //        string fileSpec = string.Empty;
        //        if (request.CurrentUploadedFiles != null)
        //        {
        //            foreach (var uploadedFile in request.CurrentUploadedFiles)
        //            {
        //                //Need to uncomment
        //                //FileInfo fi = new FileInfo(uploadedFile.FileName);
        //                //uploadedFile.FileGeneratedName = Guid.NewGuid() + fi.Extension;
        //                //fileSpec = Path.Combine(_appSettings.UploadFolderPathSilverlightDocument + @"\" + UploaderModuleEnum.TenderSupplierEFile, uploadedFile.FileGeneratedName);
        //                //if (System.IO.File.Exists(fileSpec))
        //                //    System.IO.File.Delete(fileSpec);

        //                ////Save File
        //                //using var stream = new FileStream(fileSpec, FileMode.Create);
        //                //uploadedFile.File.CopyTo(stream);
        //            }
        //        }
        //        //Need to uncomment
        //        //var tnderUser = await _service.SaveUploadFilesAsync(request);

        //        #region Error Thrower
        //        //Need to uncomment
        //        //if (tnderUser.Status == Models.TenderInfoStatusEnum.BPExpire)
        //        //{
        //        //    response.ReturnStatus = StatusCodes.Status406NotAcceptable;
        //        //    response.ReturnMessage.Add("This link is not valid anymore. Supplier Problem.");
        //        //    return BadRequest(response);
        //        //}
        //        //if (tnderUser.Status == Models.TenderInfoStatusEnum.Error)
        //        //{
        //        //    response.ReturnStatus = StatusCodes.Status406NotAcceptable;
        //        //    response.ReturnMessage.Add("This link is not valid anymore.");
        //        //    return BadRequest(response);
        //        //}
        //        //if (tnderUser.Status == Models.TenderInfoStatusEnum.NoFileToUpload)
        //        //{
        //        //    response.ReturnStatus = StatusCodes.Status417ExpectationFailed;
        //        //    response.ReturnMessage.Add("No File to upload.");
        //        //    return BadRequest(response);
        //        //}
        //        //if (tnderUser.Status == Models.TenderInfoStatusEnum.TenderClosingDateExpire)
        //        //{
        //        //    response.ReturnStatus = StatusCodes.Status406NotAcceptable;
        //        //    response.ReturnMessage.Add("This link is not valid anymore. Tender Closing Date Expired.");
        //        //    return BadRequest(response);
        //        //}
        //        //if (tnderUser.Status == Models.TenderInfoStatusEnum.TenderExpire)
        //        //{
        //        //    response.ReturnStatus = StatusCodes.Status406NotAcceptable;
        //        //    response.ReturnMessage.Add("This link is not valid anymore. Tender Expired.");
        //        //    return BadRequest(response);
        //        //}
        //        //if (tnderUser.Status == Models.TenderInfoStatusEnum.TenderFloatingDateProblem)
        //        //{
        //        //    response.ReturnStatus = StatusCodes.Status406NotAcceptable;
        //        //    response.ReturnMessage.Add("This link is not valid anymore. Tender Floating Date Problem.");
        //        //    return BadRequest(response);
        //        //}
        //        //if (tnderUser.Status == Models.TenderInfoStatusEnum.SupplierFileSubmitted)
        //        //{
        //        //    response.ReturnStatus = StatusCodes.Status406NotAcceptable;
        //        //    response.ReturnMessage.Add("This link is not valid anymore. Tender supplier already file submitted.");
        //        //    return BadRequest(response);
        //        //}

        //        //if (tnderUser.Status == Models.TenderInfoStatusEnum.Unsuccesful)
        //        //{
        //        //    response.ReturnStatus = StatusCodes.Status406NotAcceptable;
        //        //    response.ReturnMessage.Add("Something went wrong.");
        //        //    return BadRequest(response);
        //        //}

        //        #endregion


        //        response.Value = true;
        //        response.ReturnStatus = StatusCodes.Status200OK;
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            foreach (var uploadedFile in request.CurrentUploadedFiles)
        //            {
        //                //Needed to Uncomment
        //                //var fileSpec = Path.Combine(_appSettings.UploadFolderPathSilverlightDocument + @"\" + UploaderModuleEnum.TenderSupplierEFile, uploadedFile.FileGeneratedName);
        //                //if (System.IO.File.Exists(fileSpec))
        //                //    System.IO.File.Delete(fileSpec);
        //            }
        //        }
        //        catch (Exception eee)
        //        {
        //            response.ReturnStatus = StatusCodes.Status500InternalServerError;
        //            response.ReturnMessage.Add((ex.InnerException != null ? ex.InnerException.Message + Environment.NewLine + eee.Message : ex.Message));
        //            return StatusCode(StatusCodes.Status500InternalServerError, response);
        //        }
        //        response.ReturnStatus = StatusCodes.Status500InternalServerError;
        //        response.ReturnMessage.Add((ex.InnerException != null ? ex.InnerException.Message : ex.Message));
        //        return StatusCode(StatusCodes.Status500InternalServerError, response);
        //    }
        //}


        //[HttpPost]
        //[Route("get-file-download")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<IActionResult> DownloadUploadedFile([FromBody] UploadFileDownloadRequest request)
        //{
        //    return null;
        //    //TenderBPResponse response = new TenderBPResponse();
        //    //string basepathforTender = _appSettings.UploadFolderPathSilverlightDocument + @"\" + UploaderModuleEnum.TenderSupplierEFile;
        //    //TenderBPWithFile tnderbp = await _service.GetTenderBPFileAsync(eTenderCurrentEmailVersion: request.ETenderCurrentEmailVersion, tenderID: request.TenderID, bpID: request.BPID, fileID: request.FileID);
        //    //if (tnderbp == null ||
        //    //    tnderbp.PreviousFiles == null ||
        //    //    tnderbp.PreviousFiles.Count <= 0)
        //    //{
        //    //    response.ReturnStatus = StatusCodes.Status417ExpectationFailed;
        //    //    response.ReturnMessage.Add("Something went wrong. File not found.");
        //    //    return BadRequest(response);
        //    //}

        //    //var fileName = basepathforTender + @"\" + tnderbp.PreviousFiles.FirstOrDefault().FileGeneratedName;
        //    //string fullPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, fileName);

        //    //if (!System.IO.File.Exists(fullPath))
        //    //{
        //    //    response.ReturnStatus = StatusCodes.Status417ExpectationFailed;
        //    //    response.ReturnMessage.Add("Something went wrong. File not found.");
        //    //    return BadRequest(response);
        //    //}

        //    //var memory = new MemoryStream();
        //    //using (var stream = new FileStream(fullPath, FileMode.Open))
        //    //{
        //    //    await stream.CopyToAsync(memory);
        //    //}
        //    //memory.Position = 0;

        //    //return File(memory, GetContentType(fullPath));
        //}

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }

}
