using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IPartnerSignRepository
    {
        Task<PartnerSign?> GetByContractId(int contractId);
        Task<PartnerSign?> GetByContractAnnexId(int annexId);
        Task<PartnerSign?> GetByLiquidationRecordId(int liuqidationId);
        Task AddPartnerSign(PartnerSign partnerSign);
        Task UpdatePartnerSign(PartnerSign partnerSign);
    }
}
