using Coms.Domain.Entities;
using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Services
{
    public class ServiceResult
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }
    }
}
