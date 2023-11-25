using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Partners
{
    public class PartnerResult
    {
        public int Id { get; set; }

        public string? Image { get; set; }
        public string Representative { get; set; }
        public string RepresentativePosition { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string TaxCode { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }
    }
}
