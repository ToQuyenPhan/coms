using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.ContractAnnexFiles
{
    public class ContractAnnexFileObjectResult
    {
        public Guid UUID { get; set; }
        public byte[] FileData { get; set; }
        public int FileSize { get; set; }
        public DateTime UploadedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int ContractAnnexId { get; set; }
    }
}
