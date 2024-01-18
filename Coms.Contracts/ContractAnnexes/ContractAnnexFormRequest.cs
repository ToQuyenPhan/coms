using Coms.Contracts.Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace Coms.Contracts.ContractAnnexes
{
    public class ContractAnnexFormRequest
    {
        public string[] Name { get; set; }
        public string[] Value { get; set; }
        public int ContractId { get; set; }
        public int? ContractCategoryId { get; set; }
        public int ServiceId { get; set; }
        public int PartnerId { get; set; }

        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ApproveDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime SignDate { get; set; }
        public int Status { get; set; }
        public int? TemplateType { get; set; }
    }
}
