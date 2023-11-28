using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Templates;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;

namespace Coms.Application.Services.PartnerReviews
{
    public class PartnerReviewService :IPartnerReviewService
    {
        private readonly IPartnerReviewRepository _partnerReviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IPartnerRepository _partnerRepository;
        public PartnerReviewService(IPartnerReviewRepository partnerReviewRepository,
            IUserRepository userRepository,
            IContractRepository contractRepository,
            IPartnerRepository partnerRepository) 
        {
            _partnerReviewRepository = partnerReviewRepository;
            _userRepository = userRepository;
            _contractRepository = contractRepository;
            _partnerRepository = partnerRepository;
        }

        public async Task<ErrorOr<PartnerReviewResult>> AddPartnerReview(int partnerId, int userId, int contractId)
        {
            try
            {
                var user = _userRepository.GetUser(userId).Result;
                var contract =  _contractRepository.GetContract(contractId).Result;
                var partner =  _partnerRepository.GetPartner(partnerId).Result;
                var partnerReview = new PartnerReview
                {
                    ContractId = contractId,
                    PartnerId = partnerId,
                    UserId = userId,
                    IsApproved = false,
                    SendDate = DateTime.Now,
                    ReviewAt = DateTime.Now,
                    Status = PartnerReviewStatus.Active 
                };
                await _partnerReviewRepository.AddPartnerReview(partnerReview);
                var createdPartnerReview = await _partnerReviewRepository.GetPartnerReview(partnerReview.Id);
                var partnerReviewResult = new PartnerReviewResult
                {
                    Id = createdPartnerReview.Id,
                    ContractId = createdPartnerReview.Id,
                    ContractName= createdPartnerReview.Contract.ContractName,
                    IsApproved=false,
                    PartnerId= createdPartnerReview.Id,
                    PartnerCompanyName= createdPartnerReview.Partner.CompanyName,
                    ReviewAt = DateTime.Now,
                    SendDate= DateTime.Now,
                    UserId= createdPartnerReview.UserId,
                    UserName = createdPartnerReview.User.Username
                };
                return partnerReviewResult;
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<PartnerReviewResult>> ApprovePartnerReview(int id)
        {
            try
            {
                var partnerPreview = await _partnerReviewRepository.GetPartnerReview(id);
                if(partnerPreview is not null)
                {
                    partnerPreview.IsApproved = true;
                    partnerPreview.ReviewAt = DateTime.Now;
                    await _partnerReviewRepository.UpdatePartnerPreview(partnerPreview);
                    var partnerReviewResult = new PartnerReviewResult
                    {
                        Id = partnerPreview.Id,
                        ContractId = partnerPreview.Id,
                        ContractName = partnerPreview.Contract.ContractName,
                        IsApproved = partnerPreview.IsApproved,
                        PartnerId = partnerPreview.Id,
                        PartnerCompanyName = partnerPreview.Partner.CompanyName,
                        ReviewAt = partnerPreview.ReviewAt,
                        SendDate = partnerPreview.SendDate,
                        UserId = partnerPreview.UserId,
                        UserName = partnerPreview.User.Username
                    };
                    return partnerReviewResult;
                }
                else
                {
                    return Error.NotFound("404", "Partner Review is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
