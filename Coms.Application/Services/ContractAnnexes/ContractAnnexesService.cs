using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Application.Services.Contracts;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using LinqKit;
using System.Net.Mail;
using System.Net;
using System.Diagnostics.Contracts;

namespace Coms.Application.Services.ContractAnnexes
{
    public class ContractAnnexesService : IContractAnnexesService
    {
        private readonly IContractAnnexRepository _contractAnnexRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IContractFlowDetailsRepository _contractFlowDetailsRepository;
        private readonly ISystemSettingsRepository _systemSettingsRepository;
        private readonly IPartnerReviewRepository _partnerReviewRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IFlowDetailRepository _flowDetailRepository;

        public ContractAnnexesService(IContractAnnexRepository contractAnnexRepository, 
            IContractRepository contractRepository,
            IContractFlowDetailsRepository contractFlowDetailsRepository,
            ISystemSettingsRepository systemSettingsRepository,
            IPartnerReviewRepository partnerReviewRepository,
            IActionHistoryRepository actionHistoryRepository,
            IFlowDetailRepository flowDetailRepository)
        {
            _contractAnnexRepository = contractAnnexRepository;
            _contractRepository = contractRepository;
            _contractFlowDetailsRepository = contractFlowDetailsRepository;
            _systemSettingsRepository = systemSettingsRepository;
            _partnerReviewRepository = partnerReviewRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _flowDetailRepository = flowDetailRepository;
        }
        //get all contractannexes
        public async Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetContractAnnexes(string name, int? status, int currentPage, int pageSize)
        {
            ExpressionStarter<ContractAnnex> predicate = PredicateBuilder.New<ContractAnnex>(true);
            if (!string.IsNullOrEmpty(name))
            {
                string lowerCaseName = name.ToLower().Trim();
                predicate = predicate.And(c => c.ContractAnnexName.ToLower().Contains(lowerCaseName));
            }
            if (status is not null)
            {
                if (status >= 0)
                {
                    predicate = predicate.And(c => c.Status.Equals((DocumentStatus)status));
                }
            }
            IList<ContractAnnex> contractAnnexes = await _contractAnnexRepository.GetContractAnnexes();
            IList<ContractAnnex> filteredList = contractAnnexes.Where(predicate).ToList();
            int total = filteredList.Count();
            if (currentPage > 0 && pageSize > 0)
            {
                filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            IList<ContractAnnexesResult> responses = new List<ContractAnnexesResult>();
            foreach (ContractAnnex contractAnnex in filteredList)
            {
                string contractName = _contractRepository.GetContract((int)contractAnnex.ContractId).Result.ContractName;
                ContractAnnexesResult contractAnnexResult = new()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    CreatedDateString = contractAnnex.CreatedDate.ToString("dd/MM/yyyy"),
                    UpdatedDate = (DateTime)contractAnnex.UpdatedDate,
                    UpdatedDateString = ((DateTime)contractAnnex.UpdatedDate).ToString("dd/MM/yyyy"),
                    Status = contractAnnex.Status,
                    StatusString = contractAnnex.Status.ToString(),
                    ContractId = (int)contractAnnex.ContractId,
                    ContractName = contractName,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                responses.Add(contractAnnexResult);
            }
            return new
                PagingResult<ContractAnnexesResult>(responses, total, currentPage, pageSize);
        }

        //get contractannexes by contractId
        public async Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetContractAnnexesByContractId(int contractId, string name, int? status, int currentPage, int pageSize)
        {
            ExpressionStarter<ContractAnnex> predicate = PredicateBuilder.New<ContractAnnex>(true);
            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(c => c.ContractAnnexName.Contains(name.Trim()));
            }
            if (status is not null)
            {
                if (status >= 0)
                {
                    predicate = predicate.And(c => c.Status.Equals((DocumentStatus)status));
                }
            }
            IList<ContractAnnex> contractAnnexes = await _contractAnnexRepository.GetContractAnnexesByContractId(contractId);
            IList<ContractAnnex> filteredList = contractAnnexes.Where(predicate).ToList();
            int total = filteredList.Count();
            if (currentPage > 0 && pageSize > 0)
            {
                filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            IList<ContractAnnexesResult> responses = new List<ContractAnnexesResult>();
            foreach (ContractAnnex contractAnnex in filteredList)
            {
                string contractName = _contractRepository.GetContract((int)contractAnnex.ContractId).Result.ContractName;
                ContractAnnexesResult contractAnnexResult = new()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    CreatedDateString = contractAnnex.CreatedDate.ToString("dd/MM/yyyy"),
                    UpdatedDate = (DateTime)contractAnnex.UpdatedDate,
                    UpdatedDateString = ((DateTime)contractAnnex.UpdatedDate).ToString("dd/MM/yyyy"),
                    Status = contractAnnex.Status,
                    StatusString = contractAnnex.Status.ToString(),
                    ContractId = (int)contractAnnex.ContractId,
                    ContractName = contractName,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                responses.Add(contractAnnexResult);
            }
            return new
                PagingResult<ContractAnnexesResult>(responses, total, currentPage, pageSize);
        }
        //get contractannexes by contractAnnexId
        public async Task<ErrorOr<ContractAnnexesResult>> GetContractAnnexesById(int id)
        {
            ContractAnnex? contractAnnex = await _contractAnnexRepository.GetContractAnnexesById(id);
            if (contractAnnex is not null)
            {
                string contractName = _contractRepository.GetContract((int)contractAnnex.ContractId).Result.ContractName;
                ContractAnnexesResult contractAnnexResult = new()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    CreatedDateString = contractAnnex.CreatedDate.ToString("dd/MM/yyyy"),
                    Status = contractAnnex.Status,
                    StatusString = contractAnnex.Status.ToString(),
                    ContractId = (int)contractAnnex.ContractId,
                    ContractName = contractName,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                if (contractAnnex.UpdatedDate is not null)
                {
                    contractAnnexResult.UpdatedDate = (DateTime)contractAnnex.UpdatedDate;
                    contractAnnexResult.UpdatedDateString = contractAnnex.UpdatedDate.ToString();
                }
                return contractAnnexResult;
            }
            else
            {
                return Error.NotFound("404", "ContractAnnex is not found!");
            }
        }

        //delete contractannexes by contractAnnexId
        public async Task<ErrorOr<ContractAnnexesResult>> DeleteContractAnnex(int id)
        {
            ContractAnnex? contractAnnex = await _contractAnnexRepository.GetContractAnnexesById(id);
            if (contractAnnex is not null)
            {
                contractAnnex.Status = DocumentStatus.Deleted;
                await _contractAnnexRepository.UpdateContractAnnexes(contractAnnex);
                string contractName = _contractRepository.GetContract((int)contractAnnex.ContractId).Result.ContractName;
                ContractAnnexesResult contractAnnexResult = new()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    CreatedDateString = contractAnnex.CreatedDate.ToString("dd/MM/yyyy"),
                    UpdatedDate = (DateTime)contractAnnex.UpdatedDate,
                    UpdatedDateString = ((DateTime)contractAnnex.UpdatedDate).ToString("dd/MM/yyyy"),
                    Status = contractAnnex.Status,
                    StatusString = contractAnnex.Status.ToString(),
                    ContractId = (int)contractAnnex.ContractId,
                    ContractName = contractName,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                return contractAnnexResult;
            }
            else
            {
                return Error.NotFound("404", "ContractAnnex is not found!");
            }
        }

        // get your contractannexes userid name code version status isyours currentpage pagesize
        public async Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetYourContractAnnexes(int userId,
                           string name, int? status, bool isYours, int currentPage, int pageSize)
        {
            IList<ContractAnnex> contractAnnexes = new List<ContractAnnex>();
            var createAction = await _actionHistoryRepository.GetContractAnnexCreateActionByUserId(userId);
            if (createAction is not null)
            {
                foreach (var action in createAction)
                {
                    var contractAnnex = await _contractAnnexRepository.GetContractAnnexesById((int)action.ContractAnnexId);
                    if (contractAnnex is not null)
                    {
                        if (!contractAnnex.Status.Equals(DocumentStatus.Deleted))
                        {
                            if (!string.IsNullOrEmpty(contractAnnex.Link))
                            {
                                contractAnnexes.Add(contractAnnex);
                            }
                        }
                    }
                }
            }
            if (!isYours)
            {
                var flowDetails = await _flowDetailRepository.GetUserFlowDetailsByUserId(userId);
                if (flowDetails is not null)
                {
                    foreach (var flowDetail in flowDetails)
                    {
                        var contractAnnexFlowDetails = await _contractFlowDetailsRepository.GetByFlowDetailId(flowDetail.Id);
                        if (contractAnnexFlowDetails is not null)
                        {
                            
                            foreach (var contractAnnexFlowDetail in contractAnnexFlowDetails)
                            {
                                if (contractAnnexFlowDetail.ContractAnnexId is null)
                                {
                                    continue;
                                }
                                var contractAnnex = await _contractAnnexRepository.GetContractAnnexesById((int)contractAnnexFlowDetail.ContractAnnexId);
                                if (contractAnnex is not null)
                                {
                                    if (!contractAnnex.Status.Equals(DocumentStatus.Deleted))
                                    {
                                        var existedContractAnnex = contractAnnexes.FirstOrDefault(c => c.Id.Equals(contractAnnex.Id));
                                        if (existedContractAnnex is null)
                                        {
                                            if (!string.IsNullOrEmpty(contractAnnex.Link))
                                            {
                                                contractAnnexes.Add(contractAnnex);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (contractAnnexes.Count() > 0)
            {
                var predicate = PredicateBuilder.New<ContractAnnex>(true);
                predicate = predicate.And(c => c.Status != DocumentStatus.Deleted);
                if (!string.IsNullOrEmpty(name))
                {
                    predicate = predicate.And(c => c.ContractAnnexName.Contains(name.Trim(), System.StringComparison.CurrentCultureIgnoreCase));
                }
                if (status is not null)
                {
                    if (status >= 0)
                    {
                        predicate = predicate.And(c => c.Status.Equals((DocumentStatus)status));
                    }
                }
                IList<ContractAnnex> filteredList = contractAnnexes.Where(predicate).OrderByDescending(c => c.CreatedDate)
                        .ToList();
                var total = filteredList.Count();
                if (currentPage > 0 && pageSize > 0)
                {
                    filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
                }
                IList<ContractAnnexesResult> responses = new List<ContractAnnexesResult>();
                foreach (var contractAnnex in filteredList)
                {
                    string contractName = _contractRepository.GetContract((int)contractAnnex.ContractId).Result.ContractName;
                    ContractAnnexesResult contractAnnexResult = new()
                    {
                        Id = contractAnnex.Id,
                        ContractAnnexName = contractAnnex.ContractAnnexName,
                        Version = contractAnnex.Version,
                        CreatedDate = contractAnnex.CreatedDate,
                        CreatedDateString = contractAnnex.CreatedDate.ToString("dd/MM/yyyy"),
                        Status = contractAnnex.Status,
                        StatusString = contractAnnex.Status.ToString(),
                        ContractId = (int)contractAnnex.ContractId,
                        ContractName = contractName,
                        Code = contractAnnex.Code,
                        Link = contractAnnex.Link
                    };
                    if (contractAnnex.UpdatedDate is not null)
                    {
                        contractAnnexResult.UpdatedDate = (DateTime)contractAnnex.UpdatedDate;
                        contractAnnexResult.UpdatedDateString = contractAnnex.UpdatedDate.ToString();
                    }
                    var actionHistory = await _actionHistoryRepository.GetCreateActionByContractAnnexId(contractAnnex.Id);
                    if (actionHistory is not null)
                    {
                        contractAnnexResult.CreatorId = actionHistory.UserId;
                        contractAnnexResult.CreatorName = actionHistory.User.FullName;
                        contractAnnexResult.CreatorEmail = actionHistory.User.Email;
                        contractAnnexResult.CreatorImage = actionHistory.User.Image;
                    }
                    responses.Add(contractAnnexResult);
                }
                return new
                    PagingResult<ContractAnnexesResult>(responses, total, currentPage, pageSize);
            }
            else
            {
                return new PagingResult<ContractAnnexesResult>(new List<ContractAnnexesResult>(), 0, currentPage,
                                       pageSize);
            }
        }

        

        //get manager contract annexes with userid name status currentpage pagesize
        public async Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetManagerContractAnnexes(int userId,
                           string name, int status, int currentPage, int pageSize)
        {
            IList<ContractAnnex> contractAnnexes = new List<ContractAnnex>();
            var flowDetails = await _flowDetailRepository.GetUserFlowDetailsByUserId(userId);
            if (flowDetails is not null)
            {
                foreach (var flowDetail in flowDetails)
                {
                    var contractAnnexFlowDetails = await _contractFlowDetailsRepository.GetContractAnnexByFlowDetailId(flowDetail.Id);
                    if (contractAnnexFlowDetails is not null)
                    {
                        foreach (var contractAnnexFlowDetail in contractAnnexFlowDetails)
                        {
                            if (contractAnnexFlowDetail.ContractAnnexId is null)
                            {
                                continue;
                            }
                            var contractAnnex = await _contractAnnexRepository.GetContractAnnexesById((int)contractAnnexFlowDetail.ContractAnnexId);
                            if (contractAnnex is not null)
                            {
                                if (!contractAnnex.Status.Equals(DocumentStatus.Deleted))
                                {
                                    var existedContractAnnex = contractAnnexes.FirstOrDefault(c => c.Id.Equals(contractAnnex.Id));
                                    if (existedContractAnnex is null)
                                    {
                                        if (!string.IsNullOrEmpty(contractAnnex.Link))
                                        {
                                            contractAnnexes.Add(contractAnnex);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (contractAnnexes.Count() > 0)
            {
                var predicate = PredicateBuilder.New<ContractAnnex>(true);
                predicate = predicate.And(c => c.Status != DocumentStatus.Deleted);
                if (!string.IsNullOrEmpty(name))
                {
                    predicate = predicate.And(c => c.ContractAnnexName.Contains(name.Trim(), StringComparison.CurrentCultureIgnoreCase));
                }
                if (status >= 0)
                {
                    predicate = predicate.And(c => c.Status.Equals((DocumentStatus)status));
                }
                IList<ContractAnnex> filteredList = contractAnnexes.Where(predicate).OrderByDescending(c => c.CreatedDate)
                        .ToList();
                var total = filteredList.Count();
                if (currentPage > 0 && pageSize > 0)
                {
                    filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
                }
                IList<ContractAnnexesResult> responses = new List<ContractAnnexesResult>();
                foreach (var contractAnnex in filteredList)
                {
                    string contractName = _contractRepository.GetContract((int)contractAnnex.ContractId).Result.ContractName;
                    ContractAnnexesResult contractAnnexResult = new()
                    {
                        Id = contractAnnex.Id,
                        ContractAnnexName = contractAnnex.ContractAnnexName,
                        Version = contractAnnex.Version,
                        CreatedDate = contractAnnex.CreatedDate,
                        CreatedDateString = contractAnnex.CreatedDate.ToString("dd/MM/yyyy"),
                        UpdatedDate = (DateTime)contractAnnex.UpdatedDate,
                        UpdatedDateString = ((DateTime)contractAnnex.UpdatedDate).ToString("dd/MM/yyyy"),
                        Status = contractAnnex.Status,
                        StatusString = contractAnnex.Status.ToString(),
                        ContractId = (int)contractAnnex.ContractId,
                        ContractName = contractName,
                        Code = contractAnnex.Code,
                        Link = contractAnnex.Link
                    };
                    var actionHistory = await _actionHistoryRepository.GetCreateActionByContractAnnexId(contractAnnex.Id);
                    if (actionHistory is not null)
                    {
                        contractAnnexResult.CreatorId = actionHistory.UserId;
                        contractAnnexResult.CreatorName = actionHistory.User.FullName;
                        contractAnnexResult.CreatorEmail = actionHistory.User.Email;
                        contractAnnexResult.CreatorImage = actionHistory.User.Image;
                    }
                    responses.Add(contractAnnexResult);
                }
                return new
                    PagingResult<ContractAnnexesResult>(responses, total, currentPage, pageSize);
            }
            else
            {
                return new PagingResult<ContractAnnexesResult>(new List<ContractAnnexesResult>(), 0, currentPage,
                                                          pageSize);
            }
        }
        

        //approve or reject contractannexes by contractAnnexId
        public async Task<ErrorOr<ContractAnnexesResult>> ApproveContractAnnex(int contractAnnexId, int userId, bool isApproved)
        {
            try
            {
                var isSentToPartner = false;
                int yourOrder = 0;
                var approvers = await _contractFlowDetailsRepository.GetApproversByContractAnnexId(contractAnnexId);
                var yourFlowDetail = approvers.FirstOrDefault(cfd => cfd.FlowDetail.UserId.Equals(userId));
                if (!yourFlowDetail.Status.Equals(FlowDetailStatus.Waiting))
                {
                    return Error.Conflict("409", "You are already " + yourFlowDetail.Status.ToString().ToLower() + "!");
                }
                if (yourFlowDetail is not null)
                {
                    yourOrder = (int)yourFlowDetail.FlowDetail.Order;
                }
                if (yourOrder > 1)
                {
                    var previousFlowDetail = approvers.FirstOrDefault(cfd => cfd.FlowDetail.Order.Equals(yourOrder - 1));
                    if (previousFlowDetail.Status.Equals(FlowDetailStatus.Waiting))
                    {
                        return Error.Conflict("403", "The previous approver has not approved yet!");
                    }
                }
                if (yourFlowDetail.FlowDetail.Order.Equals(approvers.Count()))
                {
                    isSentToPartner = true;
                }
                if (isApproved)
                {
                    yourFlowDetail.Status = FlowDetailStatus.Approved;
                }
                else
                {
                    yourFlowDetail.Status = FlowDetailStatus.Rejected;
                }
                await _contractFlowDetailsRepository.UpdateContractAnnexFlowDetail(yourFlowDetail);
                var contractAnnex = await _contractAnnexRepository.GetContractAnnexesById(contractAnnexId);
                if (contractAnnex is not null)
                {
                    if (isApproved)
                    {
                        if (isSentToPartner)
                        {
                            contractAnnex.Status = DocumentStatus.Approved;
                            await _contractAnnexRepository.UpdateContractAnnexes(contractAnnex);
                            await SendEmail(contractAnnexId);
                        }
                    }
                    else
                    {
                        contractAnnex.Status = DocumentStatus.Rejected;
                        await _contractAnnexRepository.UpdateContractAnnexes(contractAnnex);
                    }
                    var actionHistory = new ActionHistory
                    {
                        ActionType = isApproved ? ActionType.Approved : ActionType.Rejected,
                        CreatedAt = DateTime.Now,
                        UserId = userId,
                        ContractAnnexId = contractAnnex.Id,
                    };
                    await _actionHistoryRepository.AddActionHistory(actionHistory);
                    var contractAnnexResult = new ContractAnnexesResult()
                    {
                        Id = contractAnnex.Id,
                        ContractAnnexName = contractAnnex.ContractAnnexName,
                        Version = contractAnnex.Version,
                        CreatedDate = contractAnnex.CreatedDate,
                        CreatedDateString = contractAnnex.CreatedDate.Date.ToString("dd/MM/yyyy"),
                        Status = (DocumentStatus)(int)contractAnnex.Status,
                        StatusString = contractAnnex.Status.ToString(),
                        ContractId = (int)contractAnnex.ContractId,
                        Code = contractAnnex.Code,
                        Link = contractAnnex.Link
                    };
                    if (contractAnnex.UpdatedDate is not null)
                    {
                        contractAnnexResult.UpdatedDate = (DateTime)contractAnnex.UpdatedDate;
                        contractAnnexResult.UpdatedDateString = contractAnnex.UpdatedDate.ToString();
                    }
                    return contractAnnexResult;
                }
                else
                {
                    return Error.NotFound("404", "ContractAnnex is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        
            
        }

        public async Task<ErrorOr<AuthorResult>> IsAuthor(int userId, int contractAnnexId)
        {
            var contractAnnex = await _contractAnnexRepository.GetContractAnnexesById(contractAnnexId);
            if (contractAnnex is not null)
            {
                if (contractAnnex.Status.Equals(DocumentStatus.Deleted))
                {
                    return Error.Conflict("409", "Contract Annex no longer exist!");
                }
                else
                {
                    var actionHistory = await _actionHistoryRepository.GetCreateActionByContractAnnexId(contractAnnexId);
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
                return Error.NotFound("404", "Contract Annex is not found!");
            }
        }

        private async Task SendEmail(int contractAnnexId)
        {
            var systemSettings = await _systemSettingsRepository.GetSystemSettings();
            var partnerReview = await _partnerReviewRepository.GetByContractAnnexId(contractAnnexId);
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(systemSettings.Email);
            message.To.Add(new MailAddress(partnerReview.Partner.Email));
            string bodyMessage = "You have a new contractAnnex annex to approve! Here is your code to sign in into our system: " +
                partnerReview.Partner.Code + ".";
            message.Subject = "Approve New Contract Annex";
            message.Body = bodyMessage;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(systemSettings.Email, "hibz dgyu xnww dnvx");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }

    }
}
