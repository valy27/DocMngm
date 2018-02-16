using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using DocumentManagement.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using DocumentManagement.Infrastructure.FileHelper;
using Microsoft.AspNetCore.Authorization;

namespace DocumentManagement.Controllers
{
    [Authorize(Policy = "Authenticated")]

    public class FilesController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IDocumentService _documentService;

        public FilesController(IFileService fileService, IDocumentService documentService)
        {
            _fileService = fileService;
            _documentService = documentService;
        }

        [HttpPost, DisableRequestSizeLimit, Route("api/files")]
        public async Task<IActionResult> UploadFiles(IFormFile files)
        {
            if (_fileService.UploadFiles(Request.Form.Files).IsCompletedSuccessfully)
            {
                return Ok("File successfully uploaded");
            }

            return BadRequest("File upload failure");
        }

        [HttpGet, Route("api/files")]
        public FileStreamResult DownloadFile([FromQuery] int id)
        {
            var fileData = _fileService.GetFileForDownload(id);
            if (fileData.contentType != null &&
               fileData.documentName != null &&
               fileData.fileStream != null)
            {
                return File(fileData.fileStream, fileData.contentType, fileData.documentName);
            }
            Response.StatusCode = 404;
            fileData.fileStream.Dispose();
            return null;
        }
    }
}


