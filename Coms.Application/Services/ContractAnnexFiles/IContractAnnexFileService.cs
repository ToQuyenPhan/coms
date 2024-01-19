using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.ContractAnnexFiles
{
    public interface IContractAnnexFileService
    {
        Task<ErrorOr<ContractAnnexFileObjectResult>> GetContractAnnexFileByContractAnnexId(int contractAnnexId);
    }
}
