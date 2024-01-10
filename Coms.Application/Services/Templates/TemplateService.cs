using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Enum;
using ErrorOr;
using Syncfusion.EJ2.DocumentEditor;
using System.Globalization;

namespace Coms.Application.Services.Templates
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IContractCategoryRepository _contractCategoryRepository;
        private readonly ITemplateFileRepository _templateFileRepository;

        public TemplateService(ITemplateRepository templateRepository,
                IContractCategoryRepository contractCategoryRepository,
                ITemplateFileRepository templateFileRepository)
        {
            _templateRepository = templateRepository;
            _contractCategoryRepository = contractCategoryRepository;
            _templateFileRepository = templateFileRepository;
        }

        public async Task<ErrorOr<PagingResult<TemplateResult>>> GetTemplates(string name, int? category,
                int? type, int? status, string email, int currentPage, int pageSize)
        {

            List<TemplateResult> responses = new List<TemplateResult>();
            var result = _templateRepository
                .GetTemplates(name, category, type, status, email).Result;
            if (result is not null)
            {
                foreach (var template in result)
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
                        TemplateType = (int)template.TemplateType,
                        TemplateLink = template.TemplateLink,
                        Status = (int)template.Status,
                        StatusString = template.Status.ToString(),
                        UserId = template.User.Id,
                        UserName = template.User.Username,
                        Email = template.User.Email
                    };
                    if (template.TemplateType.Equals(Domain.Enum.TemplateType.ContractAnnex))
                    {
                        templateResult.TemplateTypeString = "Contract Annex";
                    }
                    else
                    {
                        if (template.TemplateType.Equals(Domain.Enum.TemplateType.LiquidationRecord))
                        {
                            templateResult.TemplateTypeString = "Liquidation Record";
                        }
                        else
                        {
                            templateResult.TemplateTypeString = template.TemplateType.ToString();
                        }
                    }
                    responses.Add(templateResult);
                }
                var total = result.Count();
                responses = responses.OrderByDescending(t => t.CreatedDate).ToList();
                if (currentPage > 0 && pageSize > 0)
                {
                    responses = responses.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
                }
                return new
                    PagingResult<TemplateResult>(responses, total, currentPage,
                    pageSize);
            }
            else
            {
                return new PagingResult<TemplateResult>(new List<TemplateResult>(), 0, currentPage,
                    pageSize);
            }
        }

        public async Task<ErrorOr<TemplateResult>> AddTemplate(string name, string description, int category,
                int type, int status, int userId)
        {
            try
            {
                var template = new Domain.Entities.Template
                {
                    TemplateName = name,
                    Description = description,
                    ContractCategoryId = category,
                    TemplateType = (Domain.Enum.TemplateType)type,
                    TemplateLink = "",
                    CreatedDate = DateTime.Now,
                    Status = (TemplateStatus)status,
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
                    ContractCategoryId = createdTemplate.ContractCategory.Id,
                    ContractCategoryName = createdTemplate.ContractCategory.CategoryName,
                    TemplateType = (int)createdTemplate.TemplateType,
                    TemplateTypeString = createdTemplate.TemplateType.ToString(),
                    TemplateLink = createdTemplate.TemplateLink,
                    Status = (int)createdTemplate.Status,
                    StatusString = createdTemplate.Status.ToString(),
                    UserId = createdTemplate.User.Id,
                    UserName = createdTemplate.User.Username,
                    Email = createdTemplate.User.Email
                };
                return templateResult;
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<TemplateResult>> DeleteTemplate(int id)
        {
            try
            {
                if (_templateRepository.GetTemplate(id).Result is not null)
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
                        TemplateType = (int)template.TemplateType,
                        TemplateTypeString = template.TemplateType.ToString(),
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

        public async Task<ErrorOr<TemplateSfdtResult>> GetTemplate(int id)
        {
            try
            {
                var templateFile = await _templateFileRepository
                    .GetTemplateFileByTemplateId(id);
                if (templateFile is not null)
                {
                    string filePath =
                        Path.Combine(Environment.CurrentDirectory, "Templates",
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
                    stream.Dispose();
                    TemplateSfdtResult result = new TemplateSfdtResult()
                    {
                        Sfdt = sfdt
                    };
                    return result;
                }
                else
                {
                    return Error.NotFound("404", "Template file is not found!");
                }
                return Error.NotFound();
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<TemplateResult>> UpdateTemplate(string name, string description, int category,
                int type, int status, int templateId)
        {
            try
            {
                var template = await _templateRepository.GetTemplate(templateId);
                if (template is not null)
                {
                    var contractCategory = await _contractCategoryRepository
                            .GetActiveContractCategoryById(category);
                    if (contractCategory is not null)
                    {
                        template.ContractCategory = contractCategory;
                    }
                    template.TemplateType = (Domain.Enum.TemplateType)type;
                    template.TemplateName = name;
                    template.Description = description;
                    template.ContractCategoryId = category;
                    template.UpdatedDate = DateTime.Now;
                    template.Status = (TemplateStatus)status;
                    await _templateRepository.UpdateTemplate(template);
                    var templateResult = new TemplateResult
                    {
                        Id = template.Id,
                        TemplateName = template.TemplateName,
                        Description = template.Description,
                        CreatedDate = template.CreatedDate,
                        CreatedDateString = template.CreatedDate.ToString(),
                        ContractCategoryId = template.ContractCategory.Id,
                        ContractCategoryName = template.ContractCategory.CategoryName,
                        TemplateType = (int)template.TemplateType,
                        TemplateTypeString = template.TemplateType.ToString(),
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
                    return Error.NotFound("404", "Template is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<TemplateResult>> GetTemplateInformation(int id)
        {
            try
            {
                var template = await _templateRepository.GetTemplate(id);
                if (template is not null)
                {
                    var templateResult = new TemplateResult
                    {
                        Id = template.Id,
                        TemplateName = template.TemplateName,
                        Description = template.Description,
                        CreatedDate = template.CreatedDate,
                        CreatedDateString = template.CreatedDate.ToString("d MMMM yyyy",
                            CultureInfo.CreateSpecificCulture("en-US")),
                        ContractCategoryId = template.ContractCategory.Id,
                        ContractCategoryName = template.ContractCategory.CategoryName,
                        TemplateType = (int)template.TemplateType,
                        TemplateLink = template.TemplateLink,
                        Status = (int)template.Status,
                        StatusString = template.Status.ToString(),
                        UserId = template.User.Id,
                        UserName = template.User.Username,
                        Email = template.User.Email,
                        UserImage = template.User.Image
                    };
                    if (template.TemplateType.Equals(Domain.Enum.TemplateType.ContractAnnex))
                    {
                        templateResult.TemplateTypeString = "Contract Annex";
                    }
                    else
                    {
                        if (template.TemplateType.Equals(Domain.Enum.TemplateType.LiquidationRecord))
                        {
                            templateResult.TemplateTypeString = "Liquidation Record";
                        }
                        else
                        {
                            templateResult.TemplateTypeString = template.TemplateType.ToString();
                        }
                    }
                    if (template.UpdatedDate is not null)
                    {
                        templateResult.UpdatedDate = template.UpdatedDate;
                        templateResult.UpdatedDateString = AsTimeAgo((DateTime)template.UpdatedDate);
                    }
                    return templateResult;
                }
                else
                {
                    return Error.NotFound("404", "Template is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<TemplateResult>> ActivateTemplate(int id)
        {
            try
            {
                var template = await _templateRepository.GetTemplate(id);
                if (template is not null)
                {
                    var activatingTemplate = await _templateRepository.GetTemplates(string.Empty,
                        template.ContractCategoryId, (int)template.TemplateType, (int)TemplateStatus.Activating, string.Empty);
                    if (activatingTemplate is not null)
                    {
                        return Error.Conflict("409", "The contract category already has a activating " +
                            " template!");
                    }
                    else
                    {
                        template.Status = TemplateStatus.Activating;
                        await _templateRepository.UpdateTemplate(template);
                        var templateResult = new TemplateResult
                        {
                            Id = template.Id,
                            TemplateName = template.TemplateName,
                            Description = template.Description,
                            CreatedDate = template.CreatedDate,
                            CreatedDateString = template.CreatedDate.ToString(),
                            ContractCategoryId = template.ContractCategory.Id,
                            ContractCategoryName = template.ContractCategory.CategoryName,
                            TemplateType = (int)template.TemplateType,
                            TemplateTypeString = template.TemplateType.ToString(),
                            TemplateLink = template.TemplateLink,
                            Status = (int)template.Status,
                            StatusString = template.Status.ToString(),
                            UserId = template.User.Id,
                            UserName = template.User.Username,
                            Email = template.User.Email
                        };
                        if (template.UpdatedDate is not null)
                        {
                            templateResult.UpdatedDate = template.UpdatedDate;
                            templateResult.UpdatedDateString = AsTimeAgo((DateTime)template.UpdatedDate);
                        }
                        return templateResult;
                    }
                }
                else
                {
                    return Error.NotFound("404", "Template is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<TemplateResult>> DeactivateTemplate(int id)
        {
            try
            {
                var template = await _templateRepository.GetTemplate(id);
                if (template is not null)
                {
                    template.Status = TemplateStatus.Done;
                    await _templateRepository.UpdateTemplate(template);
                    var templateResult = new TemplateResult
                    {
                        Id = template.Id,
                        TemplateName = template.TemplateName,
                        Description = template.Description,
                        CreatedDate = template.CreatedDate,
                        CreatedDateString = template.CreatedDate.ToString(),
                        ContractCategoryId = template.ContractCategory.Id,
                        ContractCategoryName = template.ContractCategory.CategoryName,
                        TemplateType = (int)template.TemplateType,
                        TemplateTypeString = template.TemplateType.ToString(),
                        TemplateLink = template.TemplateLink,
                        Status = (int)template.Status,
                        StatusString = template.Status.ToString(),
                        UserId = template.User.Id,
                        UserName = template.User.Username,
                        Email = template.User.Email
                    };
                    if (template.UpdatedDate is not null)
                    {
                        templateResult.UpdatedDate = template.UpdatedDate;
                        templateResult.UpdatedDateString = AsTimeAgo((DateTime)template.UpdatedDate);
                    }
                    return templateResult;
                }
                else
                {
                    return Error.NotFound("404", "Template is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public ErrorOr<PagingResult<NotificationResult>> GetTemplateNotifications(int currentPage, int pageSize)
        {

            List<NotificationResult> responses = new List<NotificationResult>();
            var result = _templateRepository.GetAllTemplates();
            result = result.OrderByDescending(t => t.CreatedDate).ToList();
            if (result is not null)
            {
                foreach (var template in result)
                {
                    if (string.IsNullOrEmpty(template.TemplateLink))
                    {
                        continue;
                    }
                    else
                    {
                        var notificationResult = new NotificationResult()
                        {
                            Title = "New Template",
                            Message = template.User.FullName + " created new template!",
                            Long = AsTimeAgo(template.CreatedDate),
                            TemplateId = template.Id
                        };
                        responses.Add(notificationResult);
                    }
                }
                var total = result.Count();
                if (currentPage > 0 && pageSize > 0)
                {
                    responses = responses.Skip((currentPage - 1) * pageSize).Take(pageSize)
                            .ToList();
                }
                return new
                    PagingResult<NotificationResult>(responses, total, currentPage,
                    pageSize);
            }
            else
            {
                return new PagingResult<NotificationResult>(new List<NotificationResult>(), 0, currentPage,
                    pageSize);
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

        private string AsTimeAgo(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(dateTime);

            return timeSpan.TotalSeconds switch
            {
                <= 60 => $"{timeSpan.Seconds} seconds ago",

                _ => timeSpan.TotalMinutes switch
                {
                    <= 1 => "About a minute ago",
                    < 60 => $"About {timeSpan.Minutes} minutes ago",
                    _ => timeSpan.TotalHours switch
                    {
                        <= 1 => "About an hour ago",
                        < 24 => $"About {timeSpan.Hours} hours ago",
                        _ => timeSpan.TotalDays switch
                        {
                            <= 1 => "yesterday",
                            <= 30 => $"About {timeSpan.Days} days ago",

                            <= 60 => "About a month ago",
                            < 365 => $"About {timeSpan.Days / 30} months ago",

                            <= 365 * 2 => "About a year ago",
                            _ => $"About {timeSpan.Days / 365} years ago"
                        }
                    }
                }
            };
        }
    }
}
