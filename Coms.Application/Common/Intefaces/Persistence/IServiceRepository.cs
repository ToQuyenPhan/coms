using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IServiceRepository
    {
        Task<IList<Service>> GetServicesByServiceName(string serviceName);
        Task<IList<Service>> GetServices();
        Task<Service> GetServiceById(int id);
        Task AddService(Service service);
        Task UpdateService(Service service);

    }
}
