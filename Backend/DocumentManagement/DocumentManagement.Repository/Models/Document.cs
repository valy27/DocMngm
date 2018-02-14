using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DocumentManagement.Repository.Models.Identity;

namespace DocumentManagement.Repository.Models
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public double FileSize { get; set; }

        public string Descripion { get; set; }

        [ForeignKey("Account")]
        public int OwnerId { get; set; }
        public virtual Account Owner { get; set; }
    }
}