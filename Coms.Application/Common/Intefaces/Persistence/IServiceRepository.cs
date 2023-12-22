using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IServiceRepository
    {
        Task<IList<Service>> GetServicesByServiceName(string serviceName);
        Task<IList<Service>?> GetServices();
        Task<Service?> GetService(int id);
        Task AddService(Service service);
        Task UpdateService(Service service);
        Task<IList<Service>?> GetByContractCategoryId(int contractCategoryId);
    }
}
