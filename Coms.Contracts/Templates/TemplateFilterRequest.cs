using Coms.Contracts.Common.Paging;

namespace Coms.Contracts.Templates
{
    public class TemplateFilterRequest : PagingRequest
    {
        public string? TemplateName { get; set; }
        public string? Creator { get; set; }
        public int? Status { get; set; }
        public int? ContractCategoryId { get; set; }
        public int? TemplateTypeId { get; set; }
    }
}
