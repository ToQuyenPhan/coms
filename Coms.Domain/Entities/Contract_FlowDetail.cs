﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Coms.Domain.Enum;

namespace Coms.Domain.Entities
{
    public class Contract_FlowDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public FlowDetailStatus Status { get; set; }
        
        public int? ContractId { get; set; }
        public virtual Contract? Contract { get; set; }

        public int? ContractAnnexId { get; set; }
        public virtual ContractAnnex? ContractAnnex { get; set; }

        public int? LiquidationRecordId { get; set; }
        public virtual LiquidationRecord? LiquidationRecord { get; set; }

        public int FlowDetailId { get; set; }
        public virtual FlowDetail? FlowDetail { get; set; }
    }
}
