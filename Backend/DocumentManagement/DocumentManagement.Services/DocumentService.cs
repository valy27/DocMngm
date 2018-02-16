using System;
using DocumentManagement.Repository;
using DocumentManagement.Repository.Models;
using DocumentManagement.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using DocumentManagement.Infrastructure.UserResolver;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Services
{
    public class DocumentService: IDocumentService
    {
        private readonly IGenericRepository<Document> _documentRepository;
        private readonly IUserResolverService _userResolverService;
        private readonly IAccountService _accountService;
        private readonly IFileService _fileService;

        public DocumentService(IGenericRepository<Document> documentRepository, IUserResolverService userResolverService, IAccountService accountService, IFileService fileService)
        {
            _documentRepository = documentRepository;
            _userResolverService = userResolverService;
            _accountService = accountService;
            _fileService = fileService;
        }

        public void Add(Document document)
        {
            var user = _userResolverService.GetUser();
            if (user != null)
            {
                document.Created = DateTime.Now;
                document.OwnerId = _accountService.GetAccountForIdentity(user.Id).Id;
                _documentRepository.Insert(document);
            }
        }

        public IQueryable<Document> GetAll()
        {
            var user = _userResolverService.GetUser();
            if (user == null) return Enumerable.Empty<Document>().AsQueryable();
            var userRoles = _userResolverService.GetUserRoles(user).ToList();

            if (!userRoles.Any()) return Enumerable.Empty<Document>().AsQueryable();

            var documents = userRoles.Contains("Admin") ? _documentRepository.Get().Include(d => d.Owner).ThenInclude(o => o.Identity) :
                _documentRepository.Get().Where(d => d.Owner.IdentityId == user.Id).Include(d => d.Owner).ThenInclude(o => o.Identity);
            return documents?.OrderByDescending(d => d.Created);
        }

        public void Remove(int id)
        {
            var user = _userResolverService.GetUser();
            if (user == null) return;
            var userRoles = _userResolverService.GetUserRoles(user).ToList();
            if (!userRoles.Any()) return;
            if (userRoles.Contains("Admin"))
            {
                _fileService.RemoveFile($"{_documentRepository.Get().FirstOrDefault(f => f.Id == id)?.Name}");
                _documentRepository.Delete(id);
                return;
            }
            var document = _documentRepository
                .Get().FirstOrDefault(d => d.Owner.IdentityId == user.Id && d.Id == id);

            _documentRepository.Delete(document?.Id);
            _fileService.RemoveFile(document?.Name);
        }

        public bool Exists(int id)
        {
            return _documentRepository.Get().FirstOrDefault(d => d.Id == id) != null;
        }

        public bool Exists(string name)
        {
            return _documentRepository.Get().FirstOrDefault(d => d.Name == name) != null;
        }
    }
}
