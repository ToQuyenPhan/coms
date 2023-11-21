﻿using Coms.Application.Common.Intefaces.Persistence;
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
        private readonly IContractRepository _contractRepository;

        public ContractService(IAccessRepository accessRepository,
                IUserAccessRepository userAccessRepository,
                IPartnerReviewRepository partnerReviewRepository,
                IContractRepository contractRepository)
        {
            _accessRepository = accessRepository;
            _userAccessRepository = userAccessRepository;
            _partnerReviewRepository = partnerReviewRepository;
            _contractRepository = contractRepository;
        }

        public async Task<ErrorOr<ContractResult>> DeleteContract(int id)
        {
            try
            {
                if(_contractRepository.GetContract(id).Result is not null)
                {
                    var contract = await _contractRepository.GetContract(id);
                    contract.Status = DocumentStatus.Deleted;
                    await _contractRepository.UpdateContract(contract);
                    var contractResult = new ContractResult
                    {
                        Id = contract.Id,
                        ContractName = contract.ContractName,
                        Version = contract.Version,
                        CreatedDate = contract.CreatedDate,
                        CreatedDateString = contract.CreatedDate.Date.ToString("dd/MM/yyyy"),
                        UpdatedDate = contract.UpdatedDate,
                        UpdatedDateString = contract.UpdatedDate.ToString(),
                        EffectiveDate = contract.EffectiveDate,
                        EffectiveDateString = contract.EffectiveDate.ToString(),
                        Status = (int)contract.Status,
                        StatusString = contract.Status.ToString(),
                        TemplateID = contract.TemplateId,
                        Code = contract.Code,
                        Link = contract.Link,
                    };
                    return contractResult;
                }
                else
                {
                    return Error.NotFound();
                }
            }
            catch(Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
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
                predicate = predicate.And(a => a.Contract.Status != DocumentStatus.Deleted);
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
                var total = filteredList.Count();
                if(currentPage > 0 && pageSize > 0)
                {
                    filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
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
                        CreatedDateString = contract.Contract.CreatedDate.Date.ToString("dd/MM/yyyy"),
                        UpdatedDate = contract.Contract.UpdatedDate,
                        UpdatedDateString = contract.Contract.UpdatedDate.ToString(),
                        EffectiveDate = contract.Contract.EffectiveDate,
                        EffectiveDateString = contract.Contract.EffectiveDate.ToString(),
                        Status = (int)contract.Contract.Status,
                        StatusString = contract.Contract.Status.ToString(),
                        TemplateID = contract.Contract.TemplateId,
                        Code = contract.Contract.Code,
                        Link = contract.Contract.Link
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
                    PagingResult<ContractResult>(responses, total, currentPage, pageSize);
            }
            else
            {
                return new PagingResult<ContractResult>(new List<ContractResult>(), 0, currentPage,
                    pageSize);
            }
        }
    }
}