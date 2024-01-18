using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractAnnexFileRepository
    {
        Task<ContractAnnexFile?> GetContractAnnexFileById(Guid id);
        Task Update(ContractAnnexFile contractAnnexFile);
        //Add
        Task Add(ContractAnnexFile contractAnnexFile);
    }
}
