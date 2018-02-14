using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.ViewModels
{
    public class DocumentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public double FileSize { get; set; }
        public string OwnerUserName { get; set; }
        public string Description { get; set; }
    }
}
