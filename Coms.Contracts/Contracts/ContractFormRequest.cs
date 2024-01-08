using System.ComponentModel.DataAnnotations;

namespace Coms.Contracts.Contracts
{
    public class ContractFormRequest
    {
        public string[] Name {  get; set; }
        public string[] Value {  get; set; }
        public int? ContractCategoryId { get; set; }
        public int ServiceId {  get; set; }
        public int PartnerId {  get; set; }

        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime SendDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReviewDate { get; set; }
        public int Status {  get; set; }
        public int? TemplateType {  get; set; }
    }
}
