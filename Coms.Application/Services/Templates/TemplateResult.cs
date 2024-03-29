﻿namespace Coms.Application.Services.Templates
{
    public class TemplateResult
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateString { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }
        public string TemplateLink { get; set; }
        public int ContractCategoryId { get; set; }
        public string ContractCategoryName { get; set; }
        public int TemplateType { get; set; }
        public string TemplateTypeString { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserImage {  get; set; }
    }
}
