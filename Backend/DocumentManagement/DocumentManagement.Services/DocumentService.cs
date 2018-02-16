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

        public DocumentService(IGenericRepository<Document> documentRepository, IUserResolverService userResolverService, IAccountService accountService)
        {
            _documentRepository = documentRepository;
            _userResolverService = userResolverService;
            _accountService = accountService;
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
            if (user != null)
            {
                var userRoles = _userResolverService.GetUserRoles(user).Result;
                if (userRoles.Any())
                {
                    return userRoles.Contains("Admin") ? _documentRepository.Get().Include(d =>d.Owner).ThenInclude(o => o.Identity) : 
                                                                 _documentRepository.Get().Where(d => d.Owner.IdentityId == user.Id).Include(d => d.Owner).ThenInclude(o => o.Identity);
                }
            }
            return Enumerable.Empty<Document>().AsQueryable();
        }

        public void Remove(int id)
        {
            var user = _userResolverService.GetUser();
            if (user == null) return;
            var userRoles = _userResolverService.GetUserRoles(user).Result;
            if (!userRoles.Any()) return;
            if (userRoles.Contains("Admin"))
            {
                System.IO.File.Delete($@"Uploads\{_documentRepository.Get().FirstOrDefault(f=> f.Id == id)?.Name}");
                _documentRepository.Delete(id);
                return;
            }
            _documentRepository.Delete(_documentRepository
                .Get().FirstOrDefault(d => d.Owner.IdentityId == user.Id && d.Id == id)?.Id);
            System.IO.File.Delete($@"Uploads\{_documentRepository
                .Get().FirstOrDefault(d => d.Owner.IdentityId == user.Id && d.Id == id)?.Name}");
        }

        public bool Exists(int id)
        {
            return _documentRepository.GetById(id) != null;
        }

        public bool Exists(string name)
        {
            return _documentRepository.Get().Where(d => d.Name == name).FirstOrDefault() != null;
        }
    }
}
