using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Enum;
using ErrorOr;
using Syncfusion.EJ2.DocumentEditor;

namespace Coms.Application.Services.Templates
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IContractCategoryRepository _contractCategoryRepository;
        private readonly ITemplateTypeRepository _templateTypeRepository;
        private readonly ITemplateFileRepository _templateFileRepository;

        public TemplateService(ITemplateRepository templateRepository,
                IContractCategoryRepository contractCategoryRepository,
                ITemplateTypeRepository templateTypeRepository,
                ITemplateFileRepository templateFileRepository)
        {
            _templateRepository = templateRepository;
            _contractCategoryRepository = contractCategoryRepository;
            _templateTypeRepository = templateTypeRepository;
            _templateFileRepository = templateFileRepository;
        }

        public async Task<ErrorOr<PagingResult<TemplateResult>>> GetTemplates(string name, int? category, 
                int? type, int? status, string email, int currentPage, int pageSize)
        {
            if(_templateRepository
                    .GetTemplates(name, category, type, status, email, currentPage, pageSize).Result is not null)
            {
                IList<TemplateResult> responses = new List<TemplateResult>();
                var result = _templateRepository
                    .GetTemplates(name, category, type, status, email, currentPage, pageSize).Result;
                foreach(var template in result.Items)
                {
                    var templateResult = new TemplateResult
                    {
                        Id = template.Id,
                        TemplateName = template.TemplateName,
                        Description = template.Description,
                        CreatedDate = template.CreatedDate,
                        CreatedDateString = template.CreatedDate.ToString(),
                        ContractCategoryId = template.ContractCategoryId,
                        ContractCategoryName = template.ContractCategory.CategoryName,
                        TemplateTypeId = template.TemplateTypeId,
                        TemplateTypeName = template.TemplateType.Name,
                        TemplateLink = template.TemplateLink,
                        Status = (int)template.Status,
                        StatusString = template.Status.ToString(),
                        UserId = template.User.Id,
                        UserName = template.User.Username,
                        Email = template.User.Email
                    };
                    responses.Add(templateResult);
                }
                return new 
                    PagingResult<TemplateResult>(responses, result.TotalCount, result.CurrentPage, 
                    result.PageSize);
            }
            else
            {
                return new PagingResult<TemplateResult>(new List<TemplateResult>(), 0, currentPage,
                    pageSize);
            }
        }

        public async Task<ErrorOr<TemplateResult>> AddTemplate(string name, string description, int category, int type, int status, int userId)
        {
            try
            {
                var template = new Domain.Entities.Template
                {
                    TemplateName = name,
                    Description = description,
                    ContractCategoryId = category,
                    TemplateTypeId = type,
                    TemplateLink = "",
                    CreatedDate = DateTime.Now,
                    Status = (TemplateStatus) status,
                    UserId = userId
                };
                await _templateRepository.AddTemplate(template);
                var createdTemplate = await _templateRepository.GetTemplate(template.Id);
                var templateResult = new TemplateResult
                {
                    Id = createdTemplate.Id,
                    TemplateName = createdTemplate.TemplateName,
                    Description = createdTemplate.Description,
                    CreatedDate = createdTemplate.CreatedDate,
                    CreatedDateString = createdTemplate.CreatedDate.ToString(),
                    ContractCategoryId= createdTemplate.ContractCategory.Id,
                    ContractCategoryName = createdTemplate.ContractCategory.CategoryName,
                    TemplateTypeId = createdTemplate.TemplateType.Id,
                    TemplateTypeName = createdTemplate.TemplateType.Name,
                    TemplateLink = createdTemplate.TemplateLink,
                    Status = (int)createdTemplate.Status,
                    StatusString = createdTemplate.Status.ToString(),
                    UserId = createdTemplate.User.Id,
                    UserName = createdTemplate.User.Username,
                    Email = createdTemplate.User.Email
                };
                return templateResult;
            }
            catch(Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<TemplateResult>> DeleteTemplate(int id)
        {
            try
            {
                if(_templateRepository.GetTemplate(id).Result is not null)
                {
                    var template = await _templateRepository.GetTemplate(id);
                    template.Status = TemplateStatus.Deleted;
                    await _templateRepository.UpdateTemplate(template);
                    var templateResult = new TemplateResult
                    {
                        Id = template.Id,
                        TemplateName = template.TemplateName,
                        Description = template.Description,
                        CreatedDate = template.CreatedDate,
                        CreatedDateString = template.CreatedDate.ToString(),
                        ContractCategoryId = template.ContractCategoryId,
                        ContractCategoryName = template.ContractCategory.CategoryName,
                        TemplateTypeId = template.TemplateTypeId,
                        TemplateTypeName = template.TemplateType.Name,
                        TemplateLink = template.TemplateLink,
                        Status = (int)template.Status,
                        StatusString = template.Status.ToString(),
                        UserId = template.User.Id,
                        UserName = template.User.Username,
                        Email = template.User.Email
                    };
                    return templateResult;
                }
                else
                {
                    return Error.NotFound();
                } 
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<string>> GetTemplate(int templateId)
        {
            try
            {
                var templateFile = await _templateFileRepository
                    .GetTemplateFileByTemplateId(templateId);
                string filePath = 
                        Path.Combine(Environment.CurrentDirectory, "Data", 
                        templateFile.FileName + ".docx");
                var stream = File.Create(filePath);
                stream.Write(templateFile.FileData, 0, templateFile.FileData.Length);
                int index = templateFile.FileName.LastIndexOf('.');
                string type = index > -1 && index < templateFile.FileName.Length - 1 ?
                    templateFile.FileName.Substring(index) : ".docx";
                stream.Position = 0;
                WordDocument document = WordDocument.Load(stream, GetFormatType(type.ToLower()));
                string sfdt = Newtonsoft.Json.JsonConvert.SerializeObject(document);
                document.Dispose();
                stream.Close();
                TemplateSfdtResult result = new TemplateSfdtResult()
                {
                    Sfdt = sfdt
                };
                return sfdt;
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        private FormatType GetFormatType(string format)
        {
            if (string.IsNullOrEmpty(format))
                throw new NotSupportedException("This file format is not supported!");
            switch (format.ToLower())
            {
                case ".dotx":
                case ".docx":
                case ".docm":
                case ".dotm":
                    return FormatType.Docx;
                case ".dot":
                case ".doc":
                    return FormatType.Doc;
                case ".rtf":
                    return FormatType.Rtf;
                case ".txt":
                    return FormatType.Txt;
                case ".xml":
                    return FormatType.WordML;
                default:
                    throw new NotSupportedException("This file format is not supported!");
            }
        }
    }
}
