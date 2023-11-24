using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Application.Services.ContractCosts;
using Coms.Application.Services.Contracts;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        
        public async Task<ErrorOr<IList<ServiceResult>>> GetServicesByName(string? serviceName)
        {
            try
            {
                IList<Service> serviceList =new  List<Service>();
                if (serviceName == null) {
                    serviceList = _serviceRepository.GetServices().Result;
                }
                else {
                   serviceList  = _serviceRepository.GetServicesByServiceName(serviceName).Result; 
                }                            
                var results = new List<ServiceResult>();
                foreach (var service in serviceList)
                {
                    var result = new ServiceResult()
                    {
                        Id = service.Id,
                        ServiceName= service.ServiceName,
                        Description= service.Description,
                        Price= service.Price,
                        Status = (int)service.Status,
                        StatusString = service.Status.ToString(), 
                    };
                    results.Add(result);
                }
                return results;
            }
            catch (Exception ex) {         
                return Error.NotFound("Service not found!");
            }
        }
        public async Task<ErrorOr<ServiceResult>> GetService(int serviceId)
        {
            try
            {
                if (_serviceRepository.GetServiceById(serviceId).Result is not null)
                {
                    var service = await _serviceRepository.GetServiceById(serviceId);
                    var result = new ServiceResult
                    {
                        Id = service.Id,
                        ServiceName = service.ServiceName,
                        Description = service.Description,
                        Price = service.Price,
                        Status = (int)service.Status,
                        StatusString = service.Status.ToString()
                    };
                    return result;
                }
                else
                {
                    return Error.NotFound("Service not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<ServiceResult>> AddService(string serviceName, string description, double price)
        {
            try
            {
                var service = new Service
                {
                    ServiceName = serviceName,
                    Description = description,
                    Price = price,
                    Status =ServiceStatus.Active
                };
                await _serviceRepository.AddService(service);
                var created = _serviceRepository.GetServiceById(service.Id).Result;
                var result = new ServiceResult
                {
                    Id = service.Id,
                    ServiceName = service.ServiceName,
                    Description = service.Description,
                    Price = service.Price,
                    Status = (int)service.Status,
                    StatusString = service.Status.ToString()
                };
                return result;

            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
