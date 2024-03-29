﻿using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IPartnerReviewRepository
    {
        Task<PartnerReview?> GetByContractId(int contractId);
        Task<PartnerReview?> GetByContractId2(int contractId);
        Task<PartnerReview> GetPartnerReview(int id);
        Task AddPartnerReview(PartnerReview partnerReview);
        Task<IList<PartnerReview>?> GetByPartnerId(int partnerId, bool isApproved);
        Task UpdatePartnerPreview(PartnerReview partnerReview);
        Task<PartnerReview?> GetByContractAnnexId(int contractAnnexId);
        Task<PartnerReview?> GetByLiquidationRecordId(int liquidationRecordId);
        Task<IList<PartnerReview>?> GetByPartnerId(int partnerId);
    }
}
