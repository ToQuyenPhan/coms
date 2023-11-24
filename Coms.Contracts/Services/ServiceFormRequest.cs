using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.Services
{
    public class ServiceFormRequest
    {

        [Required(ErrorMessage = "ServiceName is not null")]
        [MaxLength(50)]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Description is not null")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is not null")]
        public double Price { get; set; }
    }
}
