﻿using System;
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

namespace DocumentManagement.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IUserResolverService _userResolverService;
        private readonly IDocumentService _documentService;
        private readonly IAccountService _accountService;

        public FileUploadService(IUserResolverService userResolverService, IDocumentService documentService,
            IAccountService accountService)
        {
            _userResolverService = userResolverService;
            _documentService = documentService;
            _accountService = accountService;
        }

        public async Task UploadFiles(IFormFileCollection files)
        {
            var fileInformation = files.GetFile("docInformation");
            var docInfoAsJobject = new JObject();
            using (var stream = fileInformation.OpenReadStream())
            {
                var byteResult = new byte[stream.Length];
                await stream.ReadAsync(byteResult, 0, (int) stream.Length);
                docInfoAsJobject = JObject.Parse(Encoding.UTF8.GetString(byteResult));
            }

            var file = files.GetFile("uploadFile");
            using (var stream = file.OpenReadStream())
            {
                var result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int) stream.Length);

                var user = _userResolverService.GetUser().Result;
                var namePrefix = user.Id;


                using (var fileStream = System.IO.File.Create($@"Uploads\{namePrefix}_{file.FileName}", result.Length))
                {
                    fileStream.Write(result, 0, result.Length);
                    var fileInfo = new System.IO.FileInfo($@"Uploads\{namePrefix}_{file.FileName}");
                    _documentService.Add(new Document
                    {
                        Created = DateTime.Now,
                        Descripion = docInfoAsJobject["description"].ToString(),
                        FileSize = fileInfo.Length,
                        Name = $"{namePrefix}_{file.FileName}",
                        OwnerId = _accountService.GetAccountForIdentity(user.Id).Id
                    });
                }
            }
        }

        public async Task RemoveFile(string name)
        {
            System.IO.File.Delete($@"Uploads\{name}");
        }
    }
}
