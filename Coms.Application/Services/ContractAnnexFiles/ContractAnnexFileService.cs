using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.ContractFiles;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.ContractAnnexFiles
{
    public class ContractAnnexFileService : IContractAnnexFileService
    {
        private readonly IContractAnnexFileRepository _contractAnnexFileRepository;
        private readonly IContractAnnexRepository _contractAnnexRepository;

        public ContractAnnexFileService(IContractAnnexFileRepository contractAnnexFileRepository, IContractAnnexRepository contractAnnexRepository)
        {
            _contractAnnexFileRepository = contractAnnexFileRepository;
            _contractAnnexRepository = contractAnnexRepository;
        }
        public async Task<ErrorOr<ContractAnnexFileObjectResult>> GetContractAnnexFileByContractAnnexId(int contractAnnexId)
        {
            if (_contractAnnexFileRepository.GetContractAnnexFileByContractAnnexId(contractAnnexId).Result is not null)
            {
                var result = _contractAnnexFileRepository.GetContractAnnexFileByContractAnnexId(contractAnnexId).Result;
                var response = new ContractAnnexFileObjectResult
                {
                    UUID = result.UUID,
                    FileData = result.FileData,
                    ContractAnnexId = result.ContractAnnexId,
                    FileSize = result.FileSize,
                    UploadedDate = result.UploadedDate,
                    UpdatedDate = result.UpdatedDate
                };
                return response;
            }
            else
            {
                return new ContractAnnexFileObjectResult();
            }
        }
    }
}
