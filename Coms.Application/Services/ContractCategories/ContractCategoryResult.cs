using Coms.Domain.Enum;

namespace Coms.Application.Services.ContractCategories
{
    public class ContractCategoryResult
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public ContractCategoryStatus Status { get; set; }
    }
}
