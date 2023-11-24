using Coms.Application.Services.ContractCosts;
using Coms.Application.Services.Contracts;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Services
{
    public interface IServiceService
    {
        Task<ErrorOr<ServiceResult>> GetService(int serviceId);
        Task<ErrorOr<IList<ServiceResult>>> GetServicesByName(string serviceName);
        Task<ErrorOr<ServiceResult>> AddService(string serviceName,string description, double price);
    }
}
