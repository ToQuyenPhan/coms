using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using Firebase.Storage;
using LinqKit;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;

namespace Coms.Application.Services.Contracts
{
    public class ContractService : IContractService
    {
        private string Bucket = "coms-64e4a.appspot.com";
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
        private readonly IContractFlowDetailsRepository _contractFlowDetailsRepository;
        private readonly IFlowDetailRepository _flowDetailRepository;
        private readonly IContractFieldRepository _contractFieldRepository;
        private readonly IFlowRepository _flowRepository;
        private readonly IContractFileRepository _contractFileRepository;

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
                IContractFlowDetailsRepository contractFlowDetailsRepository,
                IFlowDetailRepository flowDetailRepository,
                IContractFieldRepository contractFieldRepository,
                IFlowRepository flowRepository,
                IContractFileRepository contractFileRepository)
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
            _contractFlowDetailsRepository = contractFlowDetailsRepository;
            _flowDetailRepository = flowDetailRepository;
            _contractFieldRepository = contractFieldRepository;
            _flowRepository = flowRepository;
            _contractFileRepository = contractFileRepository;
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
                string name, string creatorName, int? status, bool isYours, int currentPage, int pageSize)
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
            if (!isYours)
            {
                var flowDetails = await _flowDetailRepository.GetUserFlowDetailsByUserId(userId);
                if (flowDetails is not null)
                {
                    foreach (var flowDetail in flowDetails)
                    {
                        var contractFlowDetails = await _contractFlowDetailsRepository.GetByFlowDetailId(flowDetail.Id);
                        if (contractFlowDetails is not null)
                        {
                            foreach (var contractFlowDetail in contractFlowDetails)
                            {
                                var contract = await _contractRepository.GetContract(contractFlowDetail.ContractId);
                                if (!contract.Status.Equals(DocumentStatus.Deleted))
                                {
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
                    if (contract.UpdatedDate is not null)
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

        public async Task<ErrorOr<int>> AddContract(string[] names, string[] values, int contractCategoryId,
                int serviceId, DateTime effectiveDate, int status, int userId, DateTime sendDate, DateTime reviewDate,
                int partnerId)
        {
            try
            {
                var namesAndValues = names.Zip(values, (n, v) => new { Name = n, Value = v });
                var template = await _templateRepository.GetTemplateByContractCategoryId(contractCategoryId);
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
                    var contract = new Contract
                    {
                        TemplateId = template.Id,
                        Link = "",
                        CreatedDate = DateTime.Now,
                        EffectiveDate = effectiveDate,
                        Version = 1,
                        Status = (DocumentStatus)status
                    };
                    foreach (var nav in namesAndValues)
                    {
                        if (nav.Name.Equals("Contract Title"))
                        {
                            contract.ContractName = nav.Value;
                        }
                        if (nav.Name.Equals("Contract Code"))
                        {
                            contract.Code = nav.Value;
                        }
                    }
                    await _contractRepository.AddContract(contract);
                    Guid UUID = new Guid();
                    var contractFile = new ContractFile()
                    {
                        UUID = UUID,
                        FileData = stream.ToArray(),
                        UploadedDate = DateTime.Now,
                        ContractId = contract.Id,
                        FileSize = stream.ToArray().Length,
                    };
                    await _contractFileRepository.Add(contractFile);
                    contractFilePath = Path.Combine(Environment.CurrentDirectory, "Contracts", contract.Id + ".pdf");
                    FileStream fileStream = new FileStream(contractFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                    pdfDocument.Save(fileStream);
                    //Closes the Word document
                    pdfDocument.Close();
                    fileStream.Close();
                    List<ContractField> contractFields = new List<ContractField>();
                    foreach (var nav in namesAndValues)
                    {
                        var contractField = new ContractField()
                        {
                            FieldName = nav.Name,
                            Content = nav.Value,
                            ContractId = contract.Id,
                        };
                        contractFields.Add(contractField);
                    }
                    await _contractFieldRepository.AddRangeContractField(contractFields);
                    var contractCost = new ContractCost()
                    {
                        ServiceId = serviceId,
                        ContractId = contract.Id
                    };
                    await _contractCostRepository.AddContractCost(contractCost);
                    var partnerReview = new PartnerReview()
                    {
                        PartnerId = partnerId,
                        UserId = userId,
                        ContractId = contract.Id,
                        SendDate = sendDate,
                        ReviewAt = reviewDate,
                        IsApproved = false,
                        Status = PartnerReviewStatus.Active
                    };
                    await _partnerReviewRepository.AddPartnerReview(partnerReview);
                    var flow = await _flowRepository.GetByContractCategoryId(contractCategoryId);
                    var flowDetails = await _flowDetailRepository.GetByFlowId(flow.Id);
                    List<Contract_FlowDetail> contractFlowDetails = new List<Contract_FlowDetail>();
                    foreach (var flowDetail in flowDetails)
                    {
                        var contractFlowDetail = new Contract_FlowDetail()
                        {
                            FlowDetailId = flowDetail.Id,
                            ContractId = contract.Id,
                            Status = FlowDetailStatus.Waiting
                        };
                        contractFlowDetails.Add(contractFlowDetail);
                    }
                    await _contractFlowDetailsRepository.AddRangeContractFlowDetails(contractFlowDetails);
                    var actionHistory = new ActionHistory()
                    {
                        ActionType = ActionType.Created,
                        CreatedAt = DateTime.Now,
                        UserId = userId,
                        ContractId = contract.Id
                    };
                    await _actionHistoryRepository.AddActionHistory(actionHistory);
                    return contract.Id;
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

        public async Task<ErrorOr<string>> UploadContract(int contractId)
        {
            try
            {
                string contractFilePath = Path.Combine(Environment.CurrentDirectory, "Contracts", contractId + ".pdf");
                var stream = File.Open(contractFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                var task = new FirebaseStorage(Bucket)
                    .Child("contracts")
                    .Child(contractId + ".pdf")
                    .PutAsync(stream);
                string link = "https://firebasestorage.googleapis.com/v0/b/coms-64e4a.appspot.com/o/contracts%2F" + contractId
                    + ".pdf?alt=media&token=451cd9c9-b548-48f3-b69c-0129a0c0836c";
                var contract = await _contractRepository.GetContract(contractId);
                contract.Link = link;
                await _contractRepository.UpdateContract(contract);
                var downloadUrl = await task;
                return link;
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<MemoryStream>> PreviewContract(string[] names, string[] values, int contractCategoryId)
        {
            try
            {
                var template = await _templateRepository.GetTemplateByContractCategoryId(contractCategoryId);
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

        public async Task<ErrorOr<PagingResult<ContractResult>>> GetManagerContracts(int userId,
                string name, string creatorName, int? status, int currentPage, int pageSize)
        {
            IList<Contract> contracts = new List<Contract>();
            var flowDetails = await _flowDetailRepository.GetUserFlowDetailsByUserId(userId);
            if (flowDetails is not null)
            {
                foreach (var flowDetail in flowDetails)
                {
                    var contractFlowDetails = await _contractFlowDetailsRepository.GetByFlowDetailId(flowDetail.Id);
                    if (contractFlowDetails is not null)
                    {
                        foreach (var contractFlowDetail in contractFlowDetails)
                        {
                            var contract = await _contractRepository.GetContract(contractFlowDetail.ContractId);
                            if (!contract.Status.Equals(DocumentStatus.Deleted))
                            {
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
                    var contract = await _contractRepository.GetContract(review.ContractId);
                    if (!string.IsNullOrEmpty(contract.Link))
                    {
                        contracts.Add(contract);
                    }
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

        public async Task<ErrorOr<ContractResult>> ApproveContract(int contractId, int userId, bool isApproved)
        {
            try
            {
                var isSentToPartner = false;
                int yourOrder = 0;
                var approvers = await _contractFlowDetailsRepository.GetApproversByContractId(contractId);
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
                await _contractFlowDetailsRepository.UpdateContractFlowDetail(yourFlowDetail);
                var contract = await _contractRepository.GetContract(contractId);
                if (contract is not null)
                {
                    if (isApproved)
                    {
                        if (isSentToPartner)
                        {
                            contract.Status = DocumentStatus.Approved;
                            await _contractRepository.UpdateContract(contract);
                        }
                    }
                    else
                    {
                        contract.Status = DocumentStatus.Rejected;
                        await _contractRepository.UpdateContract(contract);
                    }
                    var actionHistory = new ActionHistory
                    {
                        ActionType = isApproved ? ActionType.Approved : ActionType.Rejected,
                        CreatedAt = DateTime.Now,
                        UserId = userId,
                        ContractId = contract.Id,
                    };
                    await _actionHistoryRepository.AddActionHistory(actionHistory);
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

        public async Task<ErrorOr<AuthorResult>> IsAuthor(int userId, int contractId)
        {
            var contract = await _contractRepository.GetContract(contractId);
            if (contract is not null)
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
