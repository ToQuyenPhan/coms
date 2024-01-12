using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ContractFlowDetailsRepository : IContractFlowDetailsRepository
    {
        private readonly IGenericRepository<Contract_FlowDetail> _genericRepository;

        public ContractFlowDetailsRepository(IGenericRepository<Contract_FlowDetail> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Contract_FlowDetail?> GetByContractIdAndFlowDetailId(int contractId, int flowDetailId)
        {
            return await _genericRepository.FirstOrDefaultAsync(ufd => ufd.ContractId.Equals(contractId) && 
                ufd.FlowDetailId.Equals(flowDetailId), 
                new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.Contract, ufd => ufd.FlowDetail });
        }

        public async Task<IList<Contract_FlowDetail>?> GetByFlowDetailId(int flowDetailId)
        {
            var list = await _genericRepository.WhereAsync(ufd =>
                ufd.FlowDetailId.Equals(flowDetailId) && ufd.ContractId != null,
                new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.Contract, ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<Contract_FlowDetail>?> GetByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(ufd =>ufd.ContractId.Equals(contractId) && ufd.ContractId != null
            , new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.Contract, ufd => ufd.ContractAnnex, ufd => ufd.LiquidationRecord, ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }

        public async Task AddRangeContractFlowDetails(List<Contract_FlowDetail> contractFlowDetails)
        {
            await _genericRepository.CreateRangeAsync(contractFlowDetails);
        }

        public async Task UpdateContractFlowDetail(Contract_FlowDetail contractFlowDetail)
        {
            await _genericRepository.UpdateAsync(contractFlowDetail);
        }

        public async Task<IList<Contract_FlowDetail>?> GetApproversByContractId(int contractId)
        {
            var list = await _genericRepository.WhereAsync(ufd =>
                ufd.ContractId.Equals(contractId) && ufd.FlowDetail.FlowRole.Equals(FlowRole.Approver), 
                new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] 
                { ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<Contract_FlowDetail>?> GetByContractAnnexId(int contractAnnexId)
        {
            var list = await _genericRepository.WhereAsync(ufd => ufd.ContractAnnexId.Equals(contractAnnexId) && ufd.ContractAnnexId != null
            , new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.Contract, ufd => ufd.ContractAnnex, ufd => ufd.LiquidationRecord, ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<Contract_FlowDetail>?> GetByLiquidationRecordId(int liquidationRecordId)
        {
            var list = await _genericRepository.WhereAsync(ufd => ufd.LiquidationRecordId.Equals(liquidationRecordId) && ufd.LiquidationRecordId != null
            , new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.Contract, ufd => ufd.ContractAnnex, ufd => ufd.LiquidationRecord, ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<Contract_FlowDetail>?> GetByContractIdForSign(int contractId)
        {
            var list = await _genericRepository.WhereAsync(ufd => ufd.ContractId.Equals(contractId) && ufd.ContractId != null
            , new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<Contract_FlowDetail>?> GetByContractAnnexIdForSign(int contractAnnexId)
        {
            var list = await _genericRepository.WhereAsync(ufd => ufd.ContractAnnexId.Equals(contractAnnexId) && ufd.ContractAnnexId != null
            , new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }

        public async Task<IList<Contract_FlowDetail>?> GetByLiquidationRecordForSign(int liquidationRecordId)
        {
            var list = await _genericRepository.WhereAsync(ufd => ufd.LiquidationRecordId.Equals(liquidationRecordId) && ufd.LiquidationRecordId != null
            , new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }
        public async Task AddContractFlowDetail(Contract_FlowDetail contractFlowDetail)
        {
            await _genericRepository.CreateAsync(contractFlowDetail);
        }
        //get contract annex flow detail by flow detail id
        public async Task<IList<Contract_FlowDetail>?> GetContractAnnexByFlowDetailId(int flowDetailId)
        {
            var list = await _genericRepository.WhereAsync(ufd =>
                           ufd.FlowDetailId.Equals(flowDetailId) && ufd.ContractAnnexId != null,
                                          new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.Contract, ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }
        ////get by contract annex id
        //public async Task<IList<Contract_FlowDetail>?> GetByContractAnnexId(int contractAnnexId)
        //{
        //    var list = await _genericRepository.WhereAsync(ufd =>
        //                   ufd.ContractAnnexId.Equals(contractAnnexId), new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[] { ufd => ufd.Contract, ufd => ufd.FlowDetail });
        //    return (list.Count() > 0) ? list : null;
        //}
        //get approvers by contract annex id
        public async Task<IList<Contract_FlowDetail>?> GetApproversByContractAnnexId(int contractAnnexId)
        {
            var list = await _genericRepository.WhereAsync(ufd =>
                           ufd.ContractAnnexId.Equals(contractAnnexId) && ufd.FlowDetail.FlowRole.Equals(FlowRole.Approver),
                                          new System.Linq.Expressions.Expression<Func<Contract_FlowDetail, object>>[]
                                                         { ufd => ufd.FlowDetail });
            return (list.Count() > 0) ? list : null;
        }
        //update contract annex flow detail
        public async Task UpdateContractAnnexFlowDetail(Contract_FlowDetail contractFlowDetail)
        {
            await _genericRepository.UpdateAsync(contractFlowDetail);
        }
    }
}
