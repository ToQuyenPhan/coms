﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class Contract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ContractName { get; set; } = string.Empty;

        [Required]
        public int Version { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set;}

        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        public DocumentStatus Status { get; set; }
            
        [Required]
        public int TemplateID { get; set; }
        public virtual Template? Template { get; set; }

        public int AccessId { get; set; }
        public virtual Access? Access { get; set; }
        public virtual ICollection<ActionHistory>? ActionHistories { get; set; }
        public virtual ICollection<Attachment>? Attachments { get; set; }
        public virtual ICollection<ContractAnnex>? ContractAnnexes { get; set; }
        public virtual ICollection<ContractCost>? ContractCosts { get; set; }
        public virtual ICollection<ContractTerm>? ContractTerms { get; set; }
    }
}
