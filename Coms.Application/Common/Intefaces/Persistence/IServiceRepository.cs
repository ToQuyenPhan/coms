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
        Task<IList<Service>> GetContractCostByServiceName(string serviceName);
        Task<Service> GetServiceById(int id);
        Task AddService(Service service);
    }
}
