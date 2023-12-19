using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IFlowRepository
    {
        Task<Flow?> GetByContractCategoryId(int contractCategoryId);
    }
}
