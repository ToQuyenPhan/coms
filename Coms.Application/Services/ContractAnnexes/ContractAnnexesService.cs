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
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using Firebase.Storage;
using System.Reflection;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Coms.Application.Services.ContractAnnexes
{
    public class ContractAnnexesService : IContractAnnexesService
    {
        private string Bucket = "coms-64e4a.appspot.com";
        private readonly IContractAnnexRepository _contractAnnexRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IContractFlowDetailsRepository _contractFlowDetailsRepository;
        private readonly ISystemSettingsRepository _systemSettingsRepository;
        private readonly IPartnerReviewRepository _partnerReviewRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IFlowDetailRepository _flowDetailRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IContractAnnexFileRepository _contractAnnexFileRepository;
        private readonly IContractAnnexFieldRepository _contractAnnexFieldRepository;
        private readonly IFlowRepository _flowRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public ContractAnnexesService(IContractAnnexRepository contractAnnexRepository, 
            IContractRepository contractRepository,
            IContractFlowDetailsRepository contractFlowDetailsRepository,
            ISystemSettingsRepository systemSettingsRepository,
            IPartnerReviewRepository partnerReviewRepository,
            IActionHistoryRepository actionHistoryRepository,
            IFlowDetailRepository flowDetailRepository,
            ITemplateRepository templateRepository,
            IContractAnnexFileRepository contractAnnexFileRepository,
            IContractAnnexFieldRepository contractAnnexFieldRepository,
            IFlowRepository flowRepository,
            IScheduleRepository scheduleRepository)
        {
            _contractAnnexRepository = contractAnnexRepository;
            _contractRepository = contractRepository;
            _contractFlowDetailsRepository = contractFlowDetailsRepository;
            _systemSettingsRepository = systemSettingsRepository;
            _partnerReviewRepository = partnerReviewRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _flowDetailRepository = flowDetailRepository;
            _templateRepository = templateRepository;
            _contractAnnexFileRepository = contractAnnexFileRepository;
            _contractAnnexFieldRepository = contractAnnexFieldRepository;
            _flowRepository = flowRepository;
            _scheduleRepository = scheduleRepository;
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
                var contract = _contractRepository.GetContract((int)contractAnnex.ContractId).Result;
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
                    ContractName = contract.ContractName,
                    ContractCode = contract.Code,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                if (contractAnnex.UpdatedDate is not null)
                {
                    contractAnnexResult.UpdatedDate = contractAnnex.UpdatedDate;
                    contractAnnexResult.UpdatedDateString = contractAnnex.UpdatedDate.ToString();
                }
                var partner = await _partnerReviewRepository.GetByContractId((int)contractAnnex.ContractId);
                if (partner is not null)
                {
                    contractAnnexResult.PartnerName = partner.Partner.CompanyName;
                }
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
            //check contractAnnexes is not null return list null
            if (contractAnnexes is null)
            {
                return new PagingResult<ContractAnnexesResult>(new List<ContractAnnexesResult>(), 0, currentPage,
                                                          pageSize);
            }
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
                var contract = _contractRepository.GetContract((int)contractAnnex.ContractId).Result;
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
                    ContractName = contract.ContractName,
                    ContractCode = contract.Code,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                if (contractAnnex.UpdatedDate is not null)
                {
                    contractAnnexResult.UpdatedDate = (DateTime)contractAnnex.UpdatedDate;
                    contractAnnexResult.UpdatedDateString = contractAnnex.CreatedDate.ToString("dd/MM/yyyy");
                }
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
                var contract = _contractRepository.GetContract((int)contractAnnex.ContractId).Result;
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
                    ContractName = contract.ContractName,
                    ContractCode = contract.Code,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                if (contractAnnex.UpdatedDate is not null)
                {
                    contractAnnexResult.UpdatedDate = contractAnnex.UpdatedDate;
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
                var contract = _contractRepository.GetContract((int)contractAnnex.ContractId).Result;
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
                    ContractName = contract.ContractName,
                    ContractCode = contract.Code,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                if (contractAnnex.UpdatedDate is not null)
                {
                    contractAnnexResult.UpdatedDate = contractAnnex.UpdatedDate;
                    contractAnnexResult.UpdatedDateString = contractAnnex.UpdatedDate.ToString();
                }
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
                    var contract = _contractRepository.GetContract((int)contractAnnex.ContractId).Result;
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
                        ContractName = contract.ContractName,
                        ContractCode = contract.Code,
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
                    var partner = await _partnerReviewRepository.GetByContractId((int)contractAnnex.ContractId);
                    if (partner is not null)
                    {
                        contractAnnexResult.PartnerName = partner.Partner.CompanyName;
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
                    var contract = _contractRepository.GetContract((int)contractAnnex.ContractId).Result;
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
                        ContractName = contract.ContractName,
                        ContractCode = contract.Code,
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
                    if (contractAnnex.UpdatedDate is not null)
                    {
                        contractAnnexResult.UpdatedDate = contractAnnex.UpdatedDate;
                        contractAnnexResult.UpdatedDateString = contractAnnex.UpdatedDate.ToString();
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

        public async Task<ErrorOr<int>> AddContractAnnex(string[] names, string[] values, int contractId, int contractCategoryId,
                           int serviceId, DateTime effectiveDate, int status, int userId, DateTime approveDate, DateTime signDate,
                                          int partnerId, int templateType)
        {
            try
            {
                var namesAndValues = names.Zip(values, (n, v) => new { Name = n, Value = v });
                var template = await _templateRepository.GetTemplateByContractCategoryIdAndTemplateType(contractCategoryId, templateType);
                if (template is not null)
                {
                    string contractFilePath = Path.Combine(Environment.CurrentDirectory, "Contracts");
                    bool folderExists = Directory.Exists(contractFilePath);
                    if (!folderExists)
                        Directory.CreateDirectory(contractFilePath);
                    string templateFilePath = Path.Combine(Environment.CurrentDirectory, "Templates", template.Id + ".docx");
                    //Opens the template document
                    FileStream fileStreamPath = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    Syncfusion.DocIO.DLS.WordDocument document = new Syncfusion.DocIO.DLS.WordDocument(fileStreamPath, Syncfusion.DocIO.FormatType.Automatic);
                    //Performs the mail merge
                    document.MailMerge.Execute(names, values);
                    //Instantiation of DocIORenderer for Word to PDF conversion
                    DocIORenderer render = new DocIORenderer();
                    //Sets Chart rendering Options.
                    render.Settings.ChartRenderingOptions.ImageFormat = Syncfusion.OfficeChart.ExportImageFormat.Jpeg;
                    //Converts Word document into PDF document
                    PdfDocument pdfDocument = render.ConvertToPDF(document);
                    //Releases all resources used by the Word document and DocIO Renderer objects
                    render.Dispose();
                    document.Dispose();
                    //Saves the Word document to MemoryStream
                    MemoryStream stream = new MemoryStream();
                    pdfDocument.Save(stream);
                    fileStreamPath.Close();
                    byte[] byteInfo = stream.ToArray();
                    stream.Write(byteInfo, 0, byteInfo.Length);
                    stream.Position = 0;
                    var contractAnnex = new ContractAnnex
                    {
                        TemplateId = template.Id,
                        Link = "",
                        CreatedDate = DateTime.Now,
                        Version = 1,
                        Status = (DocumentStatus)status,

                    };
                    foreach (var nav in namesAndValues)
                    {
                        if (nav.Name.Equals("Contract Annex Code"))
                        {
                            var existingCode = await _contractAnnexRepository.GetByContractAnnexCode(nav.Value);
                            if (existingCode is null)
                            {
                                contractAnnex.Code = nav.Value;
                                contractAnnex.ContractAnnexName = "PHỤ LỤC HỢP ĐỒNG";
                                contractAnnex.ContractId = contractId;
                            }
                            else
                            {
                                return Error.Conflict("409", "The contractAnnex annex is already exist!");
                            }
                        }
                    }
                    await _contractAnnexRepository.AddContractAnnex(contractAnnex);
                    Guid UUID = new Guid();
                    var contractAnnexFile = new ContractAnnexFile()
                    {
                        UUID = UUID,
                        FileData = stream.ToArray(),
                        UploadedDate = DateTime.Now,
                        ContractAnnexId = contractAnnex.Id,
                        FileSize = stream.ToArray().Length,
                    };

                    await _contractAnnexFileRepository.Add(contractAnnexFile);
                    contractFilePath = Path.Combine(Environment.CurrentDirectory, "Contracts", "Annex-" + contractAnnex.Id + ".pdf");
                    FileStream fileStream = new FileStream(contractFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                    pdfDocument.Save(fileStream);
                    //Closes the Word document
                    pdfDocument.Close();
                    fileStream.Close();
                    List<ContractAnnexField> contractAnnexFields = new List<ContractAnnexField>();
                    foreach (var nav in namesAndValues)
                    {
                        var contractAnnexField = new ContractAnnexField()
                        {
                            FieldName = nav.Name,
                            Content = nav.Value,
                            ContractAnnexId = contractAnnex.Id,
                        };
                        contractAnnexFields.Add(contractAnnexField);
                    }
                    await _contractAnnexFieldRepository.AddRangeContractAnnexField(contractAnnexFields);
                    var partnerReview = new PartnerReview()
                    {
                        PartnerId = partnerId,
                        UserId = userId,
                        ContractAnnexId = contractAnnex.Id,
                        IsApproved = false,
                        Status = PartnerReviewStatus.Active
                    };
                    await _partnerReviewRepository.AddPartnerReview(partnerReview);
                    var flow = await _flowRepository.GetByContractCategoryId(contractCategoryId);
                    var flowDetails = await _flowDetailRepository.GetByFlowId(flow.Id);
                    List<Contract_FlowDetail> contractAnnexFlowDetails = new List<Contract_FlowDetail>();
                    foreach (var flowDetail in flowDetails)
                    {
                        var contractAnnexFlowDetail = new Contract_FlowDetail()
                        {
                            FlowDetailId = flowDetail.Id,
                            ContractAnnexId = contractAnnex.Id,
                            Status = FlowDetailStatus.Waiting
                        };
                        contractAnnexFlowDetails.Add(contractAnnexFlowDetail);
                        if (flowDetail.FlowRole.Equals(FlowRole.Approver))
                        {
                            var schedule = new Schedule()
                            {
                                StartDate = DateTime.Now,
                                EndDate = approveDate,
                                ScheduleType = ScheduleType.ApprovalDate,
                                EventName = "Approve Contract Annex",
                                Description = "It's time to approve contractAnnex annex!",
                                RemindBefore = 3,
                                Status = ScheduleStatus.Active,
                                UserId = (int)flowDetail.UserId
                            };
                            await _scheduleRepository.Add(schedule);
                            Dictionary<string, string> data = ToDictionary(new { DocumentType = "Contract Annex", Id = contractAnnex.Id, Type = "Approve" });
                            string title = "New Contract Annex";
                            string body = "You have a new contractAnnex annex to approve!";
                            await SendNotification(title, body, data, flowDetail.UserId.ToString());
                        } else
                        {
                            var schedule = new Schedule()
                            {
                                StartDate = DateTime.Now,
                                EndDate = signDate,
                                ScheduleType = ScheduleType.SigningDate,
                                EventName = "Sign Contract Annex",
                                Description = "It's time to sign contractAnnex annex!",
                                RemindBefore = 3,
                                Status = ScheduleStatus.Active,
                                UserId = (int)flowDetail.UserId
                            };
                            await _scheduleRepository.Add(schedule);
                            Dictionary<string, string> data = ToDictionary(new { DocumentType = "Contract Annex", Id = contractAnnex.Id, Type = "Sign" });
                            string title = "Sign Contract Annex";
                            string body = "You have a new contractAnnex annex to sign!";
                            await SendNotification(title, body, data, flowDetail.UserId.ToString());
                        }
                    }
                    await _contractFlowDetailsRepository.AddRangeContractFlowDetails(contractAnnexFlowDetails);
                    var actionHistory = new ActionHistory()
                    {
                        ActionType = ActionType.Created,
                        CreatedAt = DateTime.Now,
                        UserId = userId,
                        ContractAnnexId = contractAnnex.Id
                    };
                    await _actionHistoryRepository.AddActionHistory(actionHistory);
                    return contractAnnex.Id;
                }
                else
                {
                    return Error.Failure("500", "No template is activating in this category!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        //preview contractannexes
        public async Task<ErrorOr<MemoryStream>> PreviewContractAnnex(string[] names, string[] values, int contractCategoryId, int templateType)
        {
            try
            {
                var template = await _templateRepository.GetTemplateByContractCategoryIdAndTemplateType(contractCategoryId, templateType);
                if (template is not null)
                {
                    string templateFilePath = Path.Combine(Environment.CurrentDirectory, "Templates", template.Id + ".docx");
                    //Opens the template document
                    FileStream fileStreamPath = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    Syncfusion.DocIO.DLS.WordDocument document = new Syncfusion.DocIO.DLS.WordDocument(fileStreamPath, Syncfusion.DocIO.FormatType.Automatic);
                    //Performs the mail merge
                    document.MailMerge.Execute(names, values);
                    //Instantiation of DocIORenderer for Word to PDF conversion
                    DocIORenderer render = new DocIORenderer();
                    //Sets Chart rendering Options.
                    render.Settings.ChartRenderingOptions.ImageFormat = Syncfusion.OfficeChart.ExportImageFormat.Jpeg;
                    //Converts Word document into PDF document
                    PdfDocument pdfDocument = render.ConvertToPDF(document);
                    //Releases all resources used by the Word document and DocIO Renderer objects
                    render.Dispose();
                    document.Dispose();
                    //Saves the Word document to MemoryStream
                    MemoryStream stream = new MemoryStream();
                    pdfDocument.Save(stream);
                    //Closes the Word document
                    pdfDocument.Close();
                    fileStreamPath.Close();
                    byte[] byteInfo = stream.ToArray();
                    stream.Write(byteInfo, 0, byteInfo.Length);
                    stream.Position = 0;
                    return stream;
                }
                else
                {
                    return Error.Failure("500", "No template is activating in this category!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }


        //Upload ContractAnnex
        public async Task<ErrorOr<string>> UploadContractAnnex(int contractAnnexId)
        {
            try
            {
                string contractAnnexFilePath = Path.Combine(Environment.CurrentDirectory, "Contracts", "Annex-" + contractAnnexId + ".pdf");
                var stream = File.Open(contractAnnexFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                var task = new FirebaseStorage(Bucket)
                    .Child("contracts")
                    .Child("annex-" + contractAnnexId + ".pdf")
                    .PutAsync(stream);
                string link = "https://firebasestorage.googleapis.com/v0/b/coms-64e4a.appspot.com/o/contracts%2Fannex-" + contractAnnexId
                    + ".pdf?alt=media&token=451cd9c9-b548-48f3-b69c-0129a0c0836c";
                var contractAnnex = await _contractAnnexRepository.GetContractAnnexesById(contractAnnexId);
                contractAnnex.Link = link;
                await _contractAnnexRepository.UpdateContractAnnexes(contractAnnex);
                var downloadUrl = await task;
                return link;
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

        //Get ContractAnnex For Partner code
        public async Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetContractAnnexForPartnerCode(int partnerId,
                string name, int? status, bool isApproved, int currentPage, int pageSize)
        {
            var reviews = await _partnerReviewRepository.GetByPartnerId(partnerId, isApproved);
            if (reviews is not null)        
            {
                var predicate = PredicateBuilder.New<ContractAnnex>(true);
                predicate = predicate.And(c => c.Status == (DocumentStatus)status);
                if (!string.IsNullOrEmpty(name))
                {
                    predicate = predicate.And(c => c.ContractAnnexName.Contains(name.Trim(), System.StringComparison.CurrentCultureIgnoreCase));
                }
                IList<ContractAnnex> contractAnnexes = new List<ContractAnnex>();
                foreach (var review in reviews)
                {
                    if(review.ContractAnnexId is not null)
                    {
                        var contractAnnex = await _contractAnnexRepository.GetContractAnnexesById((int)review.ContractAnnexId);
                        if (!string.IsNullOrEmpty(contractAnnex.Link))
                        {
                            contractAnnexes.Add(contractAnnex);
                        }
                    }
                    
                }
                IList<ContractAnnex> filteredList = contractAnnexes.Where(predicate).ToList();
                var total = filteredList.Count();
                if (currentPage > 0 && pageSize > 0)
                {
                    filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
                }
                IList<ContractAnnexesResult> responses = new List<ContractAnnexesResult>();
                foreach (var contractAnnex in filteredList)
                {
                    var contract = _contractRepository.GetContract((int)contractAnnex.ContractId).Result;
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
                        ContractName = contract.ContractName,
                        ContractCode = contract.Code,
                        Code = contractAnnex.Code,
                        Link = contractAnnex.Link
                    };
                    if (contractAnnex.UpdatedDate is not null)
                    {
                        contractAnnexResult.UpdatedDate = contractAnnex.UpdatedDate;
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
                    var partner = await _partnerReviewRepository.GetByContractId((int)contractAnnex.ContractId);
                    if (partner is not null)
                    {
                        contractAnnexResult.PartnerName = partner.Partner.CompanyName;
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

        //GetManagerContractAnnexesForSign
        public async Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetManagerContractAnnexesForSign(int userId,
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
                if (status > 0)
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
                    var contract = _contractRepository.GetContract((int)contractAnnex.ContractId).Result;
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
                        ContractName = contract.ContractName,
                        ContractCode = contract.Code,
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
                    if (contractAnnex.UpdatedDate is not null)
                    {
                        contractAnnexResult.UpdatedDate = contractAnnex.UpdatedDate;
                        contractAnnexResult.UpdatedDateString = contractAnnex.UpdatedDate.ToString();
                    }
                    var partner = await _partnerReviewRepository.GetByContractId((int)contractAnnex.ContractId);
                    if (partner is not null)
                    {
                        contractAnnexResult.PartnerName = partner.Partner.CompanyName;
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

        public async Task<ErrorOr<ContractAnnexesResult>> RejectContractAnnex(int contractAnnexId, bool isApproved)
        {
            try
            {
                var contractAnnex = await _contractAnnexRepository.GetContractAnnexesById(contractAnnexId);
                if (contractAnnex is not null)
                {
                    if (!isApproved)
                    {
                        contractAnnex.Status = DocumentStatus.Rejected;
                    }
                    await _contractAnnexRepository.UpdateContractAnnexes(contractAnnex);
                    ContractAnnexesResult contractAnnexResult = new ContractAnnexesResult()
                    {
                        Id = contractAnnex.Id,
                        ContractAnnexName = contractAnnex.ContractAnnexName,
                        Version = contractAnnex.Version,
                        CreatedDate = contractAnnex.CreatedDate,
                        CreatedDateString = contractAnnex.CreatedDate.ToString("dd/MM/yyyy"),
                        Status = contractAnnex.Status,
                        StatusString = contractAnnex.Status.ToString(),
                        ContractId = (int)contractAnnex.ContractId,
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
                    if (contractAnnex.UpdatedDate is not null)
                    {
                        contractAnnexResult.UpdatedDate = contractAnnex.UpdatedDate;
                        contractAnnexResult.UpdatedDateString = contractAnnex.UpdatedDate.ToString();
                    }
                    var partner = await _partnerReviewRepository.GetByContractId((int)contractAnnex.ContractId);
                    if (partner is not null)
                    {
                        contractAnnexResult.PartnerName = partner.Partner.CompanyName;
                    }
                    return contractAnnexResult;
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







        private async Task SendEmail(int contractAnnexId)
        {
            var systemSettings = await _systemSettingsRepository.GetSystemSettings();
            var partnerReview = await _partnerReviewRepository.GetByContractAnnexId(contractAnnexId);
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(systemSettings.Email);
            message.To.Add(new MailAddress(partnerReview.Partner.Email));
            string bodyMessage = "<div style='font-family: Arial, sans-serif;'>" +
                "<p style='font-size: 18px;'>Dear " + partnerReview.Partner.CompanyName + ",</p>" +
                "<p style='font-size: 18px;'>You have a new contractAnnex annex to approve!</p>" +
                "<p style='font-size: 18px;'>Here is your code to sign in into our system:</p>" +
                "<p style='font-size: 20px; font-weight: bold;'>Your code: " + partnerReview.Partner.Code + "</p>" +
                "<p style='font-size: 18px;'>Please login to the website: <a href='https://quanlyhopdong.hisoft.vn/partner-code' style='color: blue;'>https://quanlyhopdong.hisoft.vn/partner-code</a> to see details</p>" +
                "</div>";
            message.Subject = "Approve New Contract Annex";
            message.Body = bodyMessage;
            message.IsBodyHtml = true; // This is to notify the MailMessage that the body is in HTML
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(systemSettings.Email, "qsmt mxaf ozvl ferm");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }

        private Dictionary<string, string> ToDictionary(object obj)
        {
            var dictionary = obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null).ToString());
            return dictionary;
        }

        private async Task SendNotification(string title, string body, Dictionary<string, string> data, string uid)
        {
            var message = new FirebaseAdmin.Messaging.Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = body
                },
                Data = data,
                Topic = uid
            };
            string fileConfigPath = Path.Combine(Directory.GetCurrentDirectory(), @"coms-64e4a-firebase-adminsdk-rzqca-3869e0f5ce.json");
            //var stream = new FileStream(fileConfigPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            if (FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance is null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(fileConfigPath)
                });
            }
            await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
