using Coms.Application.Services.TemplateFiles;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.ContractFiles
{
    public interface IContractFileService
    {
        Task<ErrorOr<ContractFileResult>> Add(string name, string extension, string contenType, byte[] document,
                int size, int contractId);
        Task<ErrorOr<ContractFileResult>> ExportPDf(string content, int id);
        Task<ErrorOr<ContractFileObjectResult>> GetContracFile(int contractId);
    }
}
