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
        private readonly IContractRepository _contractRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IContractCostRepository _contractCostRepository;
        private readonly IAproveWorkflowRepository _aproveWorkflowRepository;
        private readonly IUserFlowDetailsRepository _userFlowDetailsRepository;

        public ContractService(IAccessRepository accessRepository,
                IUserAccessRepository userAccessRepository,
                IPartnerReviewRepository partnerReviewRepository,
                IContractRepository contractRepository,
                IActionHistoryRepository actionHistoryRepository,
                ITemplateRepository templateRepository,
                IUserRepository userRepository,
                IPartnerRepository partnerRepository,
                IContractCostRepository contractCostRepository,
                IAproveWorkflowRepository aproveWorkflowRepository,
                IUserFlowDetailsRepository userFlowDetailsRepository)
        {
            _accessRepository = accessRepository;
            _userAccessRepository = userAccessRepository;
            _partnerReviewRepository = partnerReviewRepository;
            _templateRepository = templateRepository;
            _userRepository = userRepository;
            _partnerRepository = partnerRepository;
            _contractCostRepository = contractCostRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _contractRepository = contractRepository;
            _aproveWorkflowRepository = aproveWorkflowRepository;
            _userFlowDetailsRepository = userFlowDetailsRepository;
        }

        public async Task<ErrorOr<ContractResult>> DeleteContract(int id)
        {
            try
            {
                var contract = await _contractRepository.GetContract(id);
                if (contract is not null)
                {
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
                    return Error.NotFound("404", "Contract is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<PagingResult<ContractResult>>> GetYourContracts(int userId,
                string name, string creatorName, int? status, int currentPage, int pageSize)
        {
            IList<Contract> contracts = new List<Contract>();
            var createAction = await _actionHistoryRepository.GetCreateActionByUserId(userId);
            if (createAction is not null)
            {
                foreach (var action in createAction)
                {
                    var contract = await _contractRepository.GetContract(action.ContractId);
                    if (contract is not null)
                    {
                        if (!string.IsNullOrEmpty(contract.Link))
                        {
                            contracts.Add(contract);
                        }
                    }
                }
            }
            var yourFlowDetails = await _userFlowDetailsRepository.GetUserFlowDetailsByUserId(userId);
            if (yourFlowDetails is not null)
            {
                foreach (var yourFlowDetail in yourFlowDetails)
                {
                    var contract = await _contractRepository.GetContract(yourFlowDetail.ContractId);
                    var existedContract = contracts.FirstOrDefault(c => c.Id.Equals(contract.Id));
                    if (existedContract is null)
                    {
                        if (!string.IsNullOrEmpty(contract.Link))
                        {
                            contracts.Add(contract);
                        }
                    }
                }
            }
            if (contracts.Count() > 0)
            {
                //if (string.IsNullOrEmpty(creatorName))
                //{
                //    creatorName = "";
                //}
                var predicate = PredicateBuilder.New<Contract>(true);
                predicate = predicate.And(c => c.Status != DocumentStatus.Deleted);
                if (!string.IsNullOrEmpty(name))
                {
                    predicate = predicate.And(c => c.ContractName.Contains(name.Trim()));
                }
                if (status is not null)
                {
                    if (status >= 0)
                    {
                        predicate = predicate.And(c => c.Status.Equals((DocumentStatus)status));
                    }
                }
                IList<Contract> filteredList = contracts.Where(predicate).OrderByDescending(c => c.CreatedDate)
                        .ToList();
                var total = filteredList.Count();
                if (currentPage > 0 && pageSize > 0)
                {
                    filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
                }
                IList<ContractResult> responses = new List<ContractResult>();
                foreach (var contract in filteredList)
                {
                    var contractResult = new ContractResult()
                    {
                        Id = contract.Id,
                        ContractName = contract.ContractName,
                        Version = contract.Version,
                        CreatedDate = contract.CreatedDate,
                        CreatedDateString = contract.CreatedDate.Date.ToString("dd/MM/yyyy"),
                        EffectiveDate = contract.EffectiveDate,
                        EffectiveDateString = contract.EffectiveDate.ToString(),
                        Status = (int)contract.Status,
                        StatusString = contract.Status.ToString(),
                        TemplateID = contract.TemplateId,
                        Code = contract.Code,
                        Link = contract.Link
                    };
                    if (contract.UpdatedDate is not null)
                    {
                        contractResult.UpdatedDate = contract.UpdatedDate;
                        contractResult.UpdatedDateString = contract.UpdatedDate.ToString();
                    }
                    var actionHistory = await _actionHistoryRepository.GetCreateActionByContractId(contract.Id);
                    if (actionHistory is not null)
                    {
                        contractResult.CreatorId = actionHistory.User.Id;
                        contractResult.CreatorName = actionHistory.User.FullName;
                        contractResult.CreatorEmail = actionHistory.User.Email;
                        contractResult.CreatorImage = actionHistory.User.Image;
                    }
                    var partner = await _partnerReviewRepository.GetByContractId(contract.Id);
                    if (partner is not null)
                    {
                        contractResult.PartnerId = partner.Partner.Id;
                        contractResult.PartnerName = partner.Partner.CompanyName;
                    }
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

        public async Task<ErrorOr<IList<GeneralReportResult>>> GetGeneralReport(int userId)
        {
            IList<GeneralReportResult> responses = new List<GeneralReportResult>();
            if (_actionHistoryRepository.GetCreateActionByUserId(userId).Result is not null)
            {
                var actionHistories = await _actionHistoryRepository.GetCreateActionByUserId(userId);
                IList<Contract> drafts = new List<Contract>();
                IList<Contract> approvedContracts = new List<Contract>();
                IList<Contract> signedContracts = new List<Contract>();
                IList<Contract> finalizedContracts = new List<Contract>();
                foreach (var actionHistory in actionHistories)
                {
                    switch ((int)actionHistory.Contract.Status)
                    {
                        case 2:
                            drafts.Add(actionHistory.Contract);
                            break;
                        case 3:
                            approvedContracts.Add(actionHistory.Contract);
                            break;
                        case 5:
                            signedContracts.Add(actionHistory.Contract);
                            break;
                        case 6:
                            finalizedContracts.Add(actionHistory.Contract);
                            break;
                        default:
                            break;
                    }
                }
                if (drafts.Count() > 0)
                {
                    var generalReportResult = new GeneralReportResult()
                    {
                        Total = drafts.Count(),
                        Status = (int)DocumentStatus.Draft,
                        StatusString = DocumentStatus.Draft.ToString(),
                        Percent = (drafts.Count() * 100 / actionHistories.Count()),
                        Title = "Draft Contracts"
                    };
                    responses.Add(generalReportResult);
                }
                else
                {
                    var generalReportResult = new GeneralReportResult()
                    {
                        Total = 0,
                        Status = (int)DocumentStatus.Draft,
                        StatusString = DocumentStatus.Draft.ToString(),
                        Percent = 0,
                        Title = "Draft Contracts"
                    };
                    responses.Add(generalReportResult);
                }
                if (approvedContracts.Count() > 0)
                {
                    var generalReportResult = new GeneralReportResult()
                    {
                        Total = approvedContracts.Count(),
                        Status = (int)DocumentStatus.Approved,
                        StatusString = DocumentStatus.Approved.ToString(),
                        Percent = (approvedContracts.Count() * 100 / actionHistories.Count()),
                        Title = "Approved Contracts"
                    };
                    responses.Add(generalReportResult);
                }
                else
                {
                    var generalReportResult = new GeneralReportResult()
                    {
                        Total = 0,
                        Status = (int)DocumentStatus.Approved,
                        StatusString = DocumentStatus.Approved.ToString(),
                        Percent = 0,
                        Title = "Approved Contracts"
                    };
                    responses.Add(generalReportResult);
                }
                if (signedContracts.Count() > 0)
                {
                    var generalReportResult = new GeneralReportResult()
                    {
                        Total = signedContracts.Count(),
                        Status = (int)DocumentStatus.Signed,
                        StatusString = DocumentStatus.Signed.ToString(),
                        Percent = (signedContracts.Count() * 100 / actionHistories.Count()),
                        Title = "Signed Contracts"
                    };
                    responses.Add(generalReportResult);
                }
                else
                {
                    var generalReportResult = new GeneralReportResult()
                    {
                        Total = 0,
                        Status = (int)DocumentStatus.Signed,
                        StatusString = DocumentStatus.Signed.ToString(),
                        Percent = 0,
                        Title = "Signed Contracts"
                    };
                    responses.Add(generalReportResult);
                }
                if (finalizedContracts.Count() > 0)
                {
                    var generalReportResult = new GeneralReportResult()
                    {
                        Total = finalizedContracts.Count(),
                        Status = (int)DocumentStatus.Finalized,
                        StatusString = DocumentStatus.Finalized.ToString(),
                        Percent = (finalizedContracts.Count() * 100 / actionHistories.Count()),
                        Title = "Finalized Contracts"
                    };
                    responses.Add(generalReportResult);
                }
                else
                {
                    var generalReportResult = new GeneralReportResult()
                    {
                        Total = 0,
                        Status = (int)DocumentStatus.Finalized,
                        StatusString = DocumentStatus.Finalized.ToString(),
                        Percent = 0,
                        Title = "Finalized Contracts"
                    };
                    responses.Add(generalReportResult);
                }
                return responses.ToList();
            }
            else
            {
                var draftReport = new GeneralReportResult()
                {
                    Total = 0,
                    Status = (int)DocumentStatus.Draft,
                    StatusString = DocumentStatus.Draft.ToString(),
                    Percent = 0,
                    Title = "Draft Contracts"
                };
                var approvedReport = new GeneralReportResult()
                {
                    Total = 0,
                    Status = (int)DocumentStatus.Approved,
                    StatusString = DocumentStatus.Approved.ToString(),
                    Percent = 0,
                    Title = "Approved Contracts"
                };
                var signedReport = new GeneralReportResult()
                {
                    Total = 0,
                    Status = (int)DocumentStatus.Signed,
                    StatusString = DocumentStatus.Signed.ToString(),
                    Percent = 0,
                    Title = "Signed Contracts"
                };
                var finalizedReport = new GeneralReportResult()
                {
                    Total = 0,
                    Status = (int)DocumentStatus.Finalized,
                    StatusString = DocumentStatus.Finalized.ToString(),
                    Percent = 0,
                    Title = "Finalized Contracts"
                };
                responses.Add(draftReport);
                responses.Add(approvedReport);
                responses.Add(signedReport);
                responses.Add(finalizedReport);
                return responses.ToList();
            }
        }

        public async Task<ErrorOr<ContractResult>> GetContract(int id)
        {
            try
            {
                var contract = await _contractRepository.GetContract(id);
                if (contract is not null)
                {
                    var contractResult = new ContractResult
                    {
                        Id = contract.Id,
                        ContractName = contract.ContractName,
                        Version = contract.Version,
                        CreatedDate = contract.CreatedDate,
                        CreatedDateString = contract.CreatedDate.Date.ToString("dd/MM/yyyy"),
                        EffectiveDate = contract.EffectiveDate,
                        EffectiveDateString = contract.EffectiveDate.ToString(),
                        Status = (int)contract.Status,
                        StatusString = contract.Status.ToString(),
                        TemplateID = contract.TemplateId,
                        Code = contract.Code,
                        Link = contract.Link
                    };
                    if(contract.UpdatedDate is not null)
                    {
                        contractResult.UpdatedDate = contract.UpdatedDate;
                        contractResult.UpdatedDateString = contract.UpdatedDate.ToString();
                    }
                    var template = await _templateRepository.GetTemplate(contract.TemplateId);
                    contractResult.ContractCategory = template.ContractCategory.CategoryName;
                    return contractResult;
                }
                else
                {
                    return Error.NotFound("404", "Contract is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<ContractResult>> AddContract(string contractName, string code, int partnerId, int authorId, int signerId, int templateId, DateTime effectiveDate,
                int[] contractCosts, int status)
        {
            try
            {
                var contract = new Contract
                {
                    ContractName = contractName,
                    Code = code,
                    TemplateId = templateId,
                    Link = " ",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    EffectiveDate = effectiveDate,
                    Version = 1,
                    Status = (DocumentStatus)status

                };
                await _contractRepository.AddContract(contract);
                //var access = new Access
                //{
                //    ContractId = contract.Id,
                //    AccessRole = AccessRole.Author
                //};
                //await _accessRepository.AddAccess(access);

                //var userAccess = new User_Access
                //{
                //    UserId = authorId,
                //    AccessId = access.Id,
                //};
                //await _userAccessRepository.AddUserAccess(userAccess);
                var partnerReview = new PartnerReview
                {
                    ContractId = contract.Id,
                    PartnerId = partnerId,
                    UserId = signerId,
                    IsApproved = false,
                    SendDate = DateTime.Now,
                    ReviewAt = DateTime.Now,
                    Status = PartnerReviewStatus.Active
                };
                await _partnerReviewRepository.AddPartnerReview(partnerReview);
                var actionHistory = new ActionHistory
                {
                    ActionType = ActionType.Created,
                    UserId = authorId,
                    CreatedAt = DateTime.Now,
                    ContractId = contract.Id,
                };
                await _actionHistoryRepository.AddActionHistory(actionHistory);
                await _contractCostRepository.AddContractCostsToContract(contractCosts, contract.Id);
                var partner = _partnerRepository.GetPartner(partnerId).Result;
                var template = _templateRepository.GetTemplate(templateId).Result;
                //var user = _userRepository.GetUser(userAccess.UserId ?? throw new InvalidOperationException("Value cannot be null")).Result;
                //var contractResult = new ContractResult
                //{
                //    Id = contract.Id,
                //    Code = contract.Code,
                //    ContractName = contract.ContractName,
                //    CreatedDate = contract.CreatedDate,
                //    CreatedDateString = contract.CreatedDate.ToString(),
                //    UpdatedDate = contract.UpdatedDate,
                //    UpdatedDateString = contract.UpdatedDate.ToString(),
                //    EffectiveDate = contract.EffectiveDate,
                //    EffectiveDateString = contract.EffectiveDate.ToString(),
                //    Link = contract.Link,
                //    Status = (int)contract.Status,
                //    StatusString = contract.Status.ToString(),
                //    CreatorId = userAccess.UserId,
                //    CreatorName = user.FullName,
                //    CreatorEmail = user.Email,
                //    CreatorImage = user.Image,
                //    TemplateID = contract.TemplateId,
                //    Version = 1,
                //    PartnerId = partnerReview.PartnerId,
                //    PartnerName = partner.CompanyName
                //};
                //return contractResult;
                return Error.NotFound();
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<PagingResult<ContractResult>>> GetManagerContracts(int userId,
                string name, string creatorName, int? status, int currentPage, int pageSize)
        {
            //if (_userAccessRepository.GetYourAccesses(userId).Result is not null)
            //{
            //    if (string.IsNullOrEmpty(creatorName))
            //    {
            //        creatorName = "";
            //    }
            //    var predicate = PredicateBuilder.New<Access>(true);
            //predicate = predicate.And(a => a.Contract.Status != DocumentStatus.Deleted);
            //if (!string.IsNullOrEmpty(name))
            //{
            //    predicate = predicate.And(a => a.Contract.ContractName.Contains(name.Trim()));
            //}
            //if (status is not null)
            //{
            //    if (status >= 0)
            //    {
            //        predicate = predicate.And(c => c.Contract.Status.Equals((DocumentStatus)status));
            //    }
            //}
            //var yourAccesses = _userAccessRepository.GetYourAccesses(userId).Result;
            //IList<Access> accesses = new List<Access>();
            //foreach (var userAccess in yourAccesses)
            //{
            //    if (userAccess.User.Email.Contains(creatorName))
            //    {
            //        var access = await _accessRepository.GetManagerAccess((int)userAccess.AccessId);
            //        if (access is not null)
            //        {
            //            var approveWorkflow = await _aproveWorkflowRepository.GetByAccessId(access.Id);
            //            if(approveWorkflow is not null)
            //            {
            //                if (approveWorkflow.Status.Equals(ApproveWorkflowStatus.Waiting))
            //                {
            //                    if (!accesses.Contains(access))
            //                    {
            //                        accesses.Add(access);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //IList<Access> filteredList = accesses.Where(predicate).ToList();
            //var total = filteredList.Count();
            //if (currentPage > 0 && pageSize > 0)
            //{
            //    filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
            //            .ToList();
            //}
            //IList<ContractResult> responses = new List<ContractResult>();
            //foreach (var contract in filteredList)
            //{
            //    var contractResult = new ContractResult()
            //    {
            //        Id = contract.Contract.Id,
            //        ContractName = contract.Contract.ContractName,
            //        Version = contract.Contract.Version,
            //        CreatedDate = contract.Contract.CreatedDate,
            //        CreatedDateString = contract.Contract.CreatedDate.Date.ToString("dd/MM/yyyy"),
            //        UpdatedDate = contract.Contract.UpdatedDate,
            //        UpdatedDateString = contract.Contract.UpdatedDate.ToString(),
            //        EffectiveDate = contract.Contract.EffectiveDate,
            //        EffectiveDateString = contract.Contract.EffectiveDate.ToString(),
            //        Status = (int)contract.Contract.Status,
            //        StatusString = contract.Contract.Status.ToString(),
            //        TemplateID = contract.Contract.TemplateId,
            //        Code = contract.Contract.Code,
            //        Link = contract.Contract.Link
            //    };
            //    var access = await _userAccessRepository.GetByAccessId(contract.Id);
            //    if (contract.AccessRole.Equals(AccessRole.Author))
            //    {
            //        contractResult.CreatorId = access.User.Id;
            //        contractResult.CreatorName = access.User.FullName;
            //        contractResult.CreatorEmail = access.User.Email;
            //        contractResult.CreatorImage = access.User.Image;
            //    }
            //    var partner = await _partnerReviewRepository.GetByContractId(contract.Contract.Id);
            //    contractResult.PartnerId = partner.Partner.Id;
            //    contractResult.PartnerName = partner.Partner.CompanyName;
            //    responses.Add(contractResult);
            //}
            //return new
            //    PagingResult<ContractResult>(responses, total, currentPage, pageSize);
            return Error.NotFound();
            //}
            //else
            //{
            //    return new PagingResult<ContractResult>(new List<ContractResult>(), 0, currentPage,
            //        pageSize);
            //}
        }

        public async Task<ErrorOr<PagingResult<ContractResult>>> GetContractForPartner(int partnerId,
                string name, string code, bool isApproved, int currentPage, int pageSize)
        {
            var reviews = await _partnerReviewRepository.GetByPartnerId(partnerId, isApproved);
            if (reviews is not null)
            {
                var predicate = PredicateBuilder.New<Contract>(true);
                predicate = predicate.And(c => c.Status == DocumentStatus.Approved);
                if (!string.IsNullOrEmpty(name))
                {
                    predicate = predicate.And(c => c.ContractName.Contains(name.Trim()));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    predicate = predicate.And(c => c.Code.Contains(code.Trim()));
                }
                IList<Contract> contracts = new List<Contract>();
                foreach (var review in reviews)
                {
                    //var contract = await _contractRepository.GetContract(review.ContractId);
                    //contracts.Add(contract);
                }
                IList<Contract> filteredList = contracts.Where(predicate).ToList();
                var total = filteredList.Count();
                if (currentPage > 0 && pageSize > 0)
                {
                    filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
                }
                IList<ContractResult> responses = new List<ContractResult>();
                foreach (var contract in filteredList)
                {
                    var contractResult = new ContractResult()
                    {
                        Id = contract.Id,
                        ContractName = contract.ContractName,
                        Version = contract.Version,
                        CreatedDate = contract.CreatedDate,
                        CreatedDateString = contract.CreatedDate.Date.ToString("dd/MM/yyyy"),
                        UpdatedDate = contract.UpdatedDate,
                        UpdatedDateString = contract.UpdatedDate.ToString(),
                        EffectiveDate = contract.EffectiveDate,
                        EffectiveDateString = contract.EffectiveDate.ToString("dd/MM/yyyy"),
                        Status = (int)contract.Status,
                        StatusString = contract.Status.ToString(),
                        TemplateID = contract.TemplateId,
                        Code = contract.Code,
                        Link = contract.Link
                    };
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

        public async Task<ErrorOr<ContractResult>> ApproveContract(int contractId, int userId,
                bool isApproved)
        {
            try
            {
                //var yourAccesses = await _userAccessRepository.GetYourAccesses(userId);
                //if (yourAccesses is not null)
                //{
                var isAuthorized = false;
                var isSentToPartner = false;
                var isPreviousApproved = true;
                //var accesses = await _accessRepository.GetAccessByContractId(contractId);
                //int totalApprovers = accesses.Where(a => a.AccessRole.Equals(AccessRole.Approver)).Count();
                //foreach (var yourAccess in yourAccesses)
                //{
                //var access = await _accessRepository.GetAccessById((int)yourAccess.AccessId);
                //if (access.AccessRole.Equals(AccessRole.Approver) && access.ContractId.Equals(contractId))
                //{
                isAuthorized = true;
                //var currentWorkflow = await _aproveWorkflowRepository.GetByAccessId(access.Id);
                //var approverAccesses = accesses.Where(a => a.AccessRole.Equals(AccessRole.Approver)).ToList();
                //foreach (var approverAccess in approverAccesses)
                //{
                //var approveWorkflow = await _aproveWorkflowRepository.GetByAccessId(approverAccess.Id);
                //if((approveWorkflow.Order < currentWorkflow.Order) && approveWorkflow.Status.Equals(ApproveWorkflowStatus.Waiting))
                //{
                //    isPreviousApproved = false;
                //    break;
                //}

                //}
                if (!isPreviousApproved)
                {
                    return Error.Conflict("403", "The previous approver has not approved yet!");
                }
                //if (currentWorkflow.Order.Equals(totalApprovers))
                //{
                //    isSentToPartner = true;
                //}
                //if (isApproved)
                //{
                //    currentWorkflow.Status = ApproveWorkflowStatus.Approved;
                //}
                //else
                //{
                //    currentWorkflow.Status = ApproveWorkflowStatus.Rejected;
                //}
                //await _aproveWorkflowRepository.UpdateApproveWorkflow(currentWorkflow);
                //}
                //}
                if (isAuthorized)
                {
                    //var contract = await _contractRepository.GetContract(contractId);
                    //if (contract is not null)
                    //{
                    //    if (isSentToPartner)
                    //    {
                    //        contract.Status = DocumentStatus.Approved;
                    //        await _contractRepository.UpdateContract(contract);
                    //    }
                    //    var actionHistory = new ActionHistory
                    //    {
                    //        ActionType = isApproved ? ActionType.Approved : ActionType.Rejected,
                    //        CreatedAt = DateTime.Now,
                    //        UserId = userId,
                    //        ContractId = contractId,
                    //    };
                    //    await _actionHistoryRepository.AddActionHistory(actionHistory);
                    //    var contractResult = new ContractResult()
                    //    {
                    //        Id = contract.Id,
                    //        ContractName = contract.ContractName,
                    //        Version = contract.Version,
                    //        CreatedDate = contract.CreatedDate,
                    //        CreatedDateString = contract.CreatedDate.Date.ToString("dd/MM/yyyy"),
                    //        UpdatedDate = contract.UpdatedDate,
                    //        UpdatedDateString = contract.UpdatedDate.ToString(),
                    //        EffectiveDate = contract.EffectiveDate,
                    //        EffectiveDateString = contract.EffectiveDate.ToString("dd/MM/yyyy"),
                    //        Status = (int)contract.Status,
                    //        StatusString = contract.Status.ToString(),
                    //        TemplateID = contract.TemplateId,
                    //        Code = contract.Code,
                    //        Link = contract.Link
                    //    };
                    //    return contractResult;
                    //}
                    //else
                    //{
                    //    return Error.NotFound("404", "Contract is not found!");
                    //}
                    return Error.NotFound();
                }
                else
                {
                    return Error.Conflict("403", "You are not authorized to approve this contract!");
                }
                //}
                //else
                //{
                //    return Error.NotFound("404", "You do not have any accesses");
                //}
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<AuthorResult>> IsAuthor(int userId, int contractId)
        {
            var contract = await _contractRepository.GetContract(contractId);
            if(contract is not null)
            {
                if (contract.Status.Equals(DocumentStatus.Deleted))
                {
                    return Error.Conflict("409", "Contract no longer exist!");
                }
                else
                {
                    var actionHistory = await _actionHistoryRepository.GetCreateActionByContractId(contractId);
                    if (actionHistory.UserId.Equals(userId))
                    {
                        return new AuthorResult() { IsAuthor = true };
                    }
                    else
                    {
                        return new AuthorResult() { IsAuthor = false };
                    }
                }
            }
            else
            {
                return Error.NotFound("404", "Contract is not found!");
            }
        }
    }
}
