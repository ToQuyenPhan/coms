using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IGenericRepository<Service> _genericRepository;

        public ServiceRepository(IGenericRepository<Service> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IList<Service>> GetServicesByServiceName(string serviceName)
        {
            var list = await _genericRepository.WhereAsync(a => a.ServiceName.ToLower().Contains(serviceName.ToLower()) 
            && a.Status == Domain.Enum.ServiceStatus.Active ,
                new System.Linq.Expressions.Expression<Func<Service, object>>[] {
                    a => a.ContractCosts});
            return (list.Count() > 0) ? list : null;
        }
        public async Task<IList<Service>> GetServices()
        {
            var list = await _genericRepository.WhereAsync(a=> a.Status == Domain.Enum.ServiceStatus.Active,
                new System.Linq.Expressions.Expression<Func<Service, object>>[] {
                    a => a.ContractCosts});
            return (list.Count() > 0) ? list : null;
        }

        public async Task<Service> GetServiceById(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(a => a.Id.Equals(id),
                    new System.Linq.Expressions.Expression<Func<Service, object>>[]
                    { a => a.ContractCosts});
        }
        public async Task AddService(Service service)
        {
            await _genericRepository.CreateAsync(service);
        }
        public async Task UpdateService(Service service)
        {
            await _genericRepository.UpdateAsync(service);
        }
    }
}
