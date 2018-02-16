using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DocumentManagement.Services.Interfaces
{
    public interface IFileService
    {
        Task<TaskStatus> UploadFiles(IFormFileCollection files);
        TaskStatus RemoveFile(string name);
        (FileStream fileStream, string contentType, string documentName) GetFileForDownload(int id);
    }
}
