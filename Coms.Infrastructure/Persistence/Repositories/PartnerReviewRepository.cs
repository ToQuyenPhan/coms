﻿using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class PartnerReviewRepository : IPartnerReviewRepository
    {
        private readonly IGenericRepository<PartnerReview> _genericRepository;

        public PartnerReviewRepository(IGenericRepository<PartnerReview> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<PartnerReview> GetByContractId(int contractId)
        {
            return await _genericRepository.FirstOrDefaultAsync(pr => pr.ContractId.Equals(contractId),
                new System.Linq.Expressions.Expression<Func<PartnerReview, object>>[] {
                    pr => pr.User, pr => pr.Contract, pr => pr.Partner });
        }

        public async Task<PartnerReview> GetPartnerReview(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(pr => pr.Id.Equals(id),
                new System.Linq.Expressions.Expression<Func<PartnerReview, object>>[] {
                    pr => pr.User, pr => pr.Contract, pr => pr.Partner });
        }

        public async Task AddPartnerReview(PartnerReview partnerReview)
        {
            await _genericRepository.CreateAsync(partnerReview);
        }

        public async Task<IList<PartnerReview>?> GetByPartnerId(int partnerId, bool isApproved)
        {
            var list = await _genericRepository.WhereAsync(pr => pr.PartnerId == partnerId &&
                    pr.IsApproved == isApproved && pr.Status.Equals(PartnerReviewStatus.Active), 
                    new System.Linq.Expressions.Expression<Func<PartnerReview, object>>[] {
                    pr => pr.User, pr => pr.Contract, pr => pr.Partner });
            return (list.Count() > 0) ? list : null;
        }

        public async Task UpdatePartnerPreview(PartnerReview partnerReview)
        {
            await _genericRepository.UpdateAsync(partnerReview);
        }
    }
}
