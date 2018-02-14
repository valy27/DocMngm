﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DocumentManagement.Repository.Models.Identity;

namespace DocumentManagement.Repository.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime Registerd { get; set; }

        [ForeignKey("ApplicationUser")]
        public string IdentityId { get; set; }
        public virtual ApplicationUser Identity { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}