using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;

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
                if (serviceList != null)
                {
                    foreach (var service in serviceList)
                    {
                        var result = new ServiceResult()
                        {
                            Id = service.Id,
                            ServiceName = service.ServiceName,
                            Description = service.Description,
                            Price = service.Price,
                            Status = (int)service.Status,
                            StatusString = service.Status.ToString(),
                        };
                        results.Add(result);
                    }
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
                if (_serviceRepository.GetService(serviceId).Result is not null)
                {
                    var service = await _serviceRepository.GetService(serviceId);
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
                var created = _serviceRepository.GetService(service.Id).Result;
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

        public async Task<ErrorOr<ServiceResult>> UpdateService(int serviceId, string serviceName, string description, double price)
        {
            try
            {
                if (_serviceRepository.GetService(serviceId).Result is not null)
                {
                    var service = await _serviceRepository.GetService(serviceId);
                    service.ServiceName = serviceName;
                    service.Description = description;
                    service.Price = price;
                    await _serviceRepository.UpdateService(service);
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

        public async Task<ErrorOr<ServiceResult>> DeleteService(int serviceId)
        {
            try
            {
                if (_serviceRepository.GetService(serviceId).Result is not null)
                {
                    var service = await _serviceRepository.GetService(serviceId);
                    service.Status = ServiceStatus.Inactive;
                    await _serviceRepository.UpdateService(service);    
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

        public async Task<ErrorOr<IList<ServiceResult>>> GetActiveServices(int? contractCategoryId)
        {
            try
            {
                var services = await _serviceRepository.GetServices();
                if (contractCategoryId.HasValue)
                {
                    services = await _serviceRepository.GetByContractCategoryId(contractCategoryId.Value);
                }
                if (services is not null)
                {
                    IList<ServiceResult> results = new List<ServiceResult>();
                    foreach(var service in services)
                    {
                        var result = new ServiceResult
                        {
                            Id = service.Id,
                            ServiceName = service.ServiceName,
                            Description = service.Description,
                            Price = service.Price,
                            Status = (int)service.Status,
                            StatusString = service.Status.ToString()
                        };
                        results.Add(result);
                    }
                    return results.ToList();
                }
                else
                {
                    return Error.NotFound("404", "Service not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
