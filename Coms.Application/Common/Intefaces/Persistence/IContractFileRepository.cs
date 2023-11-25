using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractFileRepository
    {
        Task Add(ContractFile contractFile);
        Task<ContractFile?> GetContractFileByContractId(int templateId);
    }
}
