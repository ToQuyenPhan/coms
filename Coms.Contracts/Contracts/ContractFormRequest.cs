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
        [Required(ErrorMessage = "ContractName is not null")]
        public string ContractName { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "EffectiveDate is not null")]
        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }
        [Url(ErrorMessage = "Link must be a valid URL")]
        public string Link { get; set; }

        [Required(ErrorMessage = "TemplateId is not null")]
        public int TemplateId { get; set; }

        [Required(ErrorMessage = "PartnerId is not null")]
        public int PartnerId { get; set; }
        [Required(ErrorMessage = "ServiceId is not null")]
        public int[] Services { get; set; }

    }
}
