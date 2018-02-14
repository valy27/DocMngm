using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentManagement.Repository.Models;

namespace DocumentManagement.Services.Interfaces
{
    public interface IDocumentService
    {
        void Add(Document document);
        IQueryable<Document> GetAll();
        void Remove(int id);
        bool Exists(int id);
    }
}
