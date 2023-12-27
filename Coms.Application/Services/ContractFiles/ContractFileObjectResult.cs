using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.ContractFiles
{
    public class ContractFileObjectResult
    {
        public Guid UUID { get; set; }
        public byte[] FileData { get; set; }
        public int FileSize { get; set; }
        public DateTime UploadedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int ContractId { get; set; }
    }
}
