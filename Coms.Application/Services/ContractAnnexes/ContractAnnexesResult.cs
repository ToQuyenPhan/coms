﻿using Coms.Domain.Enum;

namespace Coms.Application.Services.ContractAnnexes
{
    public class ContractAnnexesResult
    {
        public int Id { get; set; }
        public string? ContractAnnexName { get; set; }
        public int Version { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedDateString { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedDateString { get; set; }
        public string? Code { get; set; }
        public string? Link { get; set; }
        public DocumentStatus Status { get; set; }
        public string? StatusString { get; set; }
        public int ContractId { get; set; }
        public string? ContractName { get; set; }
        public string? ContractCode { get; set; }
        public string? PartnerName { get; set; }
        public int? CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorEmail { get; set; }
        public string CreatorImage { get; set; }
    }
}
