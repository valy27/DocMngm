using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DocumentManagement.Repository.Models;
using DocumentManagement.Services.Interfaces;
using DocumentManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.Controllers
{
    [Authorize(Policy = "Authenticated")]
    [Route("api/[controller]/[action]")]
    public class DocumentsController: Controller
    {
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(IDocumentService documentService, IMapper mapper, ILogger<DocumentsController> logger)
        {
            _documentService = documentService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int id)
        {
            var document = _documentService.GetAll().FirstOrDefault(d => d.Id == id);

            if (document != null)
            {
                return Ok(document);
            }
            return NotFound($"No document with Id {id}");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
           
            var documents = _documentService.GetAll().ToList();

            var mappedDocuments = documents.Select(doc =>
            {
                var mappedDoc = _mapper.Map<DocumentViewModel>(doc);
                mappedDoc.OwnerUserName = doc.Owner.Identity.UserName;
                return mappedDoc;
            });

            if (documents.Any())
            {
               // _logger.LogInformation("Get all documents");
               
                return Ok(mappedDocuments);
            }
            return NotFound("No documents for user");
        }

       

        //[HttpPost]
        //public IActionResult Create([FromBody] CreateDocumentViewModel document)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var mappedDocument = _mapper.Map<Document>(document);
            
        //    _documentService.Add(mappedDocument);

        //    return Ok("Document created");
        //}

        

        [HttpDelete]
        public IActionResult Remove([FromQuery] int id)
        {
            if (!_documentService.Exists(id))
            {
                return NotFound("Document not foud");
            }
            _documentService.Remove(id);
            return Ok();
        }
    }
}
