using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DocumentManagement.Infrastructure.UserResolver;
using DocumentManagement.Repository.Models;
using DocumentManagement.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using DocumentManagement.Infrastructure.FileHelper;
using DocumentManagement.Repository;
using Microsoft.AspNetCore.Http.Internal;

namespace DocumentManagement.Services
{
    public class FileService : IFileService
    {
        private readonly IUserResolverService _userResolverService;
        private readonly IAccountService _accountService;
        private readonly IGenericRepository<Document> _documentRepository;
        private readonly string _filesPath;

        public FileService(IUserResolverService userResolverService,
            IAccountService accountService, IGenericRepository<Document> documentRepository)
        {
            _userResolverService = userResolverService;
            _accountService = accountService;
            _filesPath = @"Uploads\";
            _documentRepository = documentRepository;
        }

        public async Task<TaskStatus> UploadFiles(IFormFileCollection files)
        {
            var fileInformation = files.GetFile("docInformation");
            var docInfoAsJobject = new JObject();
            using (var stream = fileInformation.OpenReadStream())
            {
                var byteResult = new byte[stream.Length];
                await stream.ReadAsync(byteResult, 0, (int) stream.Length);
                docInfoAsJobject = JObject.Parse(Encoding.UTF8.GetString(byteResult));
            }

            var user = _userResolverService.GetUser();
            var file = files.GetFile("uploadFile");
            using (var stream = file.OpenReadStream())
            {
                var result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int) stream.Length);

                var namePrefix = user.Id;

               if(_documentRepository.Get(d => d.Name == $"{namePrefix}_{file.FileName}").Any())
                {
                    return TaskStatus.Canceled;
                }
                using (var fileStream = System.IO.File.Create($"{_filesPath}{namePrefix}_{file.FileName}", result.Length))
                {
                    fileStream.Write(result, 0, result.Length);
                    var fileInfo = new System.IO.FileInfo($"{_filesPath}{namePrefix}_{file.FileName}");
                    _documentRepository.Insert(new Document
                    {
                        Created = DateTime.Now,
                        Descripion = docInfoAsJobject["description"].ToString(),
                        FileSize = fileInfo.Length,
                        Name = $"{namePrefix}_{file.FileName}",
                        OwnerId = _accountService.GetAccountForIdentity(user.Id).Id
                    });
                    return TaskStatus.RanToCompletion;
                }
            }
        }

        public (FileStream fileStream, string contentType, string documentName) GetFileForDownload(int id)
        {
            var docName = _documentRepository.GetById(id)?.Name;
            var path = Path.GetFullPath(Path.Combine(System.IO.Directory.GetCurrentDirectory(), $"{_filesPath}{docName}"));

            var stream = new FileStream(path, FileMode.Open);
            var result = (fileStream: stream, contentType: MIMEAssistant.GetMIMEType(docName), documentName: docName);
            return result;
        }


        public TaskStatus RemoveFile(string name)
        {
            if (name == String.Empty) return TaskStatus.Faulted;
            System.IO.File.Delete($"{_filesPath}{name}");
            return TaskStatus.RanToCompletion;
        }
    }
}