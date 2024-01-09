using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class PartnerSignRepository : IPartnerSignRepository
    {
        private readonly IGenericRepository<PartnerSign> _genericRepository;

        public PartnerSignRepository(IGenericRepository<PartnerSign> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<PartnerSign?> GetByContractId(int contractId)
        {
            return await _genericRepository.FirstOrDefaultAsync(pr => pr.ContractId.Equals(contractId) && pr.ContractId != null,
                new System.Linq.Expressions.Expression<Func<PartnerSign, object>>[] {
                    pr => pr.ContractAnnex, pr => pr.LiquidationRecord, pr => pr.Contract, pr => pr.Partner });
        }
        public async Task<PartnerSign?> GetByContractAnnexId(int annexId)
        {
            return await _genericRepository.FirstOrDefaultAsync(pr => pr.ContractAnnexId.Equals(annexId) && pr.ContractAnnexId != null,
                new System.Linq.Expressions.Expression<Func<PartnerSign, object>>[] {
                    pr => pr.ContractAnnex, pr => pr.LiquidationRecord, pr => pr.Contract, pr => pr.Partner });
        }
        public async Task<PartnerSign?> GetByLiquidationRecordId(int liuqidationId)
        {
            return await _genericRepository.FirstOrDefaultAsync(pr => pr.LiquidationRecordId.Equals(liuqidationId) && pr.LiquidationRecordId != null,
                new System.Linq.Expressions.Expression<Func<PartnerSign, object>>[] {
                    pr => pr.ContractAnnex, pr => pr.LiquidationRecord, pr => pr.Contract, pr => pr.Partner });
        }

        public async Task AddPartnerSign(PartnerSign partnerSign)
        {
            await _genericRepository.CreateAsync(partnerSign);
        }

        public async Task UpdatePartnerSign(PartnerSign partnerSign)
        {
            await _genericRepository.UpdateAsync(partnerSign);
        }
    }
}
