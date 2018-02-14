using DocumentManagement.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DocumentManagement.Controllers
{
    public class FilesController: Controller
    {
        private readonly IFileUploadService _fileService;

        public FilesController(IFileUploadService fileService)
        {
            _fileService = fileService;
        }
     //   [Authorize(Policy = "Authenticated")]
        [HttpPost, DisableRequestSizeLimit, Route("api/files")]
        public async Task<IActionResult> UploadFiles(IFormFile files)
        {
            if (_fileService.UploadFiles(Request.Form.Files).IsCompletedSuccessfully)
            {
                return Ok("File successfully uploaded");
            }

            return BadRequest("File upload failure");
        }
    }
}
