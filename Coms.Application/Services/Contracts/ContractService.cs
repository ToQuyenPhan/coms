using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using LinqKit;

namespace Coms.Application.Services.Contracts
{
    public class ContractService : IContractService
    {
        private readonly IAccessRepository _accessRepository;
        private readonly IUserAccessRepository _userAccessRepository;
        private readonly IPartnerReviewRepository _partnerReviewRepository;

        public ContractService(IAccessRepository accessRepository,
                IUserAccessRepository userAccessRepository,
                IPartnerReviewRepository partnerReviewRepository)
        {
            _accessRepository = accessRepository;
            _userAccessRepository = userAccessRepository;
            _partnerReviewRepository = partnerReviewRepository;
        }

        public async Task<ErrorOr<PagingResult<ContractResult>>> GetYourContracts(int userId, 
                string name, string creatorName, int? status, int currentPage, int pageSize)
        {
            if (_userAccessRepository.GetYourAccesses(userId).Result is not null)
            {
                if(string.IsNullOrEmpty(creatorName))
                {
                    creatorName = "";
                }
                var predicate = PredicateBuilder.New<Access>(true);
                if (!string.IsNullOrEmpty(name))
                {
                    predicate = predicate.And(a => a.Contract.ContractName.Contains(name.Trim()));
                }
                if(status is not null)
                {
                    if (status >= 0)
                    {
                        predicate = predicate.And(c => c.Contract.Status.Equals((DocumentStatus)status));
                    }
                }
                var yourAccesses = _userAccessRepository.GetYourAccesses(userId).Result;
                IList<Access> accesses = new List<Access>();
                foreach(var userAccess in yourAccesses)
                {
                    if (userAccess.User.Email.Contains(creatorName))
                    {
                        var access = await _accessRepository.GetAccessById((int)userAccess.AccessId);
                        if (!accesses.Contains(access))
                        {
                            accesses.Add(access);
                        }
                    }
                }
                IList<Access> filteredList = accesses.Where(predicate).ToList();
                if(currentPage > 0 && pageSize > 0)
                {
                    filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize);
                }
                IList<ContractResult> responses = new List<ContractResult>();
                foreach (var contract in filteredList)
                {                    
                    var contractResult = new ContractResult()
                    {
                        Id = contract.Contract.Id,
                        ContractName = contract.Contract.ContractName,
                        Version = contract.Contract.Version,
                        CreatedDate = contract.Contract.CreatedDate,
                        CreatedDateString = contract.Contract.CreatedDate.ToString(),
                        UpdatedDate = contract.Contract.UpdatedDate,
                        UpdatedDateString = contract.Contract.UpdatedDate.ToString(),
                        EffectiveDate = contract.Contract.EffectiveDate,
                        EffectiveDateString = contract.Contract.EffectiveDate.ToString(),
                        Status = (int)contract.Contract.Status,
                        StatusString = contract.Contract.Status.ToString(),
                        TemplateID = contract.Contract.TemplateId,
                    };
                    var access = await _userAccessRepository.GetByAccessId(contract.Id);
                    if (contract.AccessRole.Equals(AccessRole.Author))
                    {
                        contractResult.CreatorId = access.User.Id;
                        contractResult.CreatorName = access.User.FullName;
                        contractResult.CreatorEmail = access.User.Email;
                        contractResult.CreatorImage = access.User.Image;
                    }
                    var partner = await _partnerReviewRepository.GetByContractId(contract.Contract.Id);
                    contractResult.PartnerId = partner.Partner.Id;
                    contractResult.PartnerName = partner.Partner.CompanyName;
                    responses.Add(contractResult);
                }
                return new
                    PagingResult<ContractResult>(responses, filteredList.Count(), currentPage,
                    pageSize);
            }
            else
            {
                return new PagingResult<ContractResult>(new List<ContractResult>(), 0, currentPage,
                    pageSize);
            }
        }
    }
}
