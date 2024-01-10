using Coms.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Coms.Contracts.ContractCategories
{
    public class ContractCategoryFormRequest
    {
        [Required(ErrorMessage = "Contract Category Name is not null")]
        public string ContractCategoryName { get; set; }
        [Required]
        public ContractCategoryStatus Status { get; set;}
    }
}
