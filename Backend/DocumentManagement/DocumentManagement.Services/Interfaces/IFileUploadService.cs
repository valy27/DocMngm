using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task UploadFiles(IFormFileCollection files);
    }
}
