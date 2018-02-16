using DocumentManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DocumentManagement.Controllers
{
    [Authorize(Policy = "Authenticated")]

    public class FilesController : Controller
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost, DisableRequestSizeLimit, Route("api/files")]
        public IActionResult UploadFiles(IFormFile files)
        {

            var task = _fileService.UploadFiles(Request.Form.Files).Result;
            if (task == TaskStatus.RanToCompletion)
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
            
            if (fileData.fileStream != null) fileData.fileStream.Dispose();
            Response.StatusCode = 404;
            return null;
        }
    }
}


