﻿using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IContractAnnexRepository
    {
        //get all contractannexes
        Task<IList<ContractAnnex>> GetContractAnnexes();
        //get contractannexes by contractId
        Task<IList<ContractAnnex>> GetContractAnnexesByContractId(int contractId);
        //get contractannexes by contractAnnexId
        Task<ContractAnnex> GetContractAnnexesById(int contractAnnexId);
        //Update contractannexes by contractAnnexId
        Task UpdateContractAnnexes(ContractAnnex contractAnnex);

    }
}
