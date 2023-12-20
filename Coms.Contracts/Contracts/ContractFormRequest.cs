using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.Contracts
{
    public class ContractFormRequest
    {
        //public string ContractName { get; set; }

        //public string Code { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime EffectiveDate { get; set; }

        //public int TemplateId { get; set; }

        //public int PartnerId { get; set; }
        //public int[] Services { get; set; }
        //public int SignerId { get; set; }
        //public int Status { get; set; }
        public string[] Name {  get; set; }
        public string[] Value {  get; set; }

        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }
    }
}
