using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Application.Services.Contracts;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using LinqKit;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;

namespace Coms.Application.Services.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IPartnerReviewRepository _partnerReviewRepository;
        private readonly IContractCostRepository _contractCostRepository;
        public ServiceService(IServiceRepository serviceRepository,
            IContractRepository contractRepository,
            IPartnerReviewRepository partnerReviewRepository,
            IContractCostRepository contractCostRepository)
        {
            _serviceRepository = serviceRepository;
            _contractRepository = contractRepository;
            _partnerReviewRepository = partnerReviewRepository;
            _contractCostRepository = contractCostRepository;
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

        public async Task<ErrorOr<ServiceResult>> AddService(string serviceName, string description, double price, int contractCategoryId)
        {
            try
            {
                var service = new Service
                {
                    ServiceName = serviceName,
                    Description = description,
                    Price = price,
                    ContractCategoryId = contractCategoryId,
                    Status =ServiceStatus.Active
                };
                await _serviceRepository.AddService(service);
                var result = new ServiceResult
                {
                    Id = service.Id,
                    ServiceName = service.ServiceName,
                    Description = service.Description,
                    Price = service.Price,
                    ContractCategoryId = service.ContractCategoryId,
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

        public async Task<ErrorOr<ServiceResult>> UpdateService(int serviceId, string serviceName, string description, double price, 
                int contractCategoryId)
        {
            try
            {
                var service = await _serviceRepository.GetService(serviceId);
                if (service is not null)
                {
                    service.ServiceName = serviceName;
                    service.Description = description;
                    service.Price = price;
                    service.ContractCategoryId = contractCategoryId;
                    await _serviceRepository.UpdateService(service);
                    var result = new ServiceResult
                    {
                        Id = service.Id,
                        ServiceName = service.ServiceName,
                        Description = service.Description,
                        Price = service.Price,
                        Status = (int)service.Status,
                        ContractCategoryId = contractCategoryId,
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

        public async Task<ErrorOr<ServiceResult>> DeleteService(int id)
        {
            try
            {
                var service = await _serviceRepository.GetService(id);
                if (service is not null)
                {
                    service.Status = ServiceStatus.Inactive;
                    await _serviceRepository.UpdateService(service);    
                    var result = new ServiceResult
                    {
                        Id = service.Id,
                        ServiceName = service.ServiceName,
                        Description = service.Description,
                        Price = service.Price,
                        Status = (int)service.Status,
                        ContractCategoryId = service.ContractCategoryId,
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

        public async Task<ErrorOr<PagingResult<ServiceResult>>> GetActiveServicesWithFilter(int? contractCategoryId, string serviceName, 
                int currentPage, int pageSize)
        {
            try
            {
                var services = await _serviceRepository.GetServices();
                if (services is not null)
                {
                    IList<ServiceResult> results = new List<ServiceResult>();
                    foreach (var service in services)
                    {
                        var result = new ServiceResult
                        {
                            Id = service.Id,
                            ServiceName = service.ServiceName,
                            Description = service.Description,
                            Price = service.Price,
                            Status = (int)service.Status,
                            StatusString = service.Status.ToString(),
                            ContractCategoryId = service.ContractCategoryId,
                            ContractCategoryName = service.ContractCategory.CategoryName
                        };
                        results.Add(result);
                    }
                    if (contractCategoryId.HasValue)
                    {
                        results = results.Where(s => s.ContractCategoryId.Equals(contractCategoryId.Value)).ToList();
                    }
                    if (!string.IsNullOrEmpty(serviceName))
                    {
                        results = results.Where(s => s.ServiceName.Contains(serviceName, StringComparison.CurrentCultureIgnoreCase))
                                .ToList();
                    }
                    int total = results.Count();
                    if (currentPage > 0 && pageSize > 0)
                    {
                        results = results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                    }
                    return new PagingResult<ServiceResult>(results, total, currentPage, pageSize);
                }
                else
                {
                    return new PagingResult<ServiceResult>(new List<ServiceResult>(), 0, currentPage, pageSize);
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<PagingResult<ServiceResult>>> GetServicesByPartnerID(int? partnerId, int currentPage, int pageSize)
        {
            try
            {
                IList<Service> serviceList = _serviceRepository.GetServices().Result;
                if (serviceList.Count() > 0)
                {
                    var predicate = PredicateBuilder.New<Service>(true);
                    IList<Domain.Entities.Contract> contracts = new List<Domain.Entities.Contract>();
                    IList<Domain.Entities.ContractCost> contractCosts = new List<Domain.Entities.ContractCost>();
                    if (partnerId.HasValue)
                    {
                        var partnerReviews = await _partnerReviewRepository.GetByPartnerId((int)partnerId);

                        if (partnerReviews.Count() > 0)
                        {
                            foreach (var partnerReview in partnerReviews)
                            {
                                var contract = await _contractRepository.GetContract((int)partnerReview.ContractId);
                                contracts.Add(contract);
                            }

                        }

                        if (contracts.Count > 0)
                        {
                            foreach (var partnerReview in partnerReviews)
                            {
                                var contractCost = await _contractCostRepository.GetByContractId((int)partnerReview.ContractId);
                                contractCosts.Add(contractCost);
                            }
                        }
                    }
                    if (contractCosts.Count > 0)
                    {
                        predicate = predicate.And(c => contractCosts.Any(cc => cc != null && cc.ServiceId == c.Id));

                    }
                    else
                    {
                        predicate = predicate.And(c => false);
                    }

                    IList<Service> filteredList = serviceList.Where(predicate)
                      .ToList();
                    var results = new List<ServiceResult>();
                    if (filteredList != null)
                    {
                        foreach (var service in filteredList)
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
                    int total = results.Count();
                    if (currentPage > 0 && pageSize > 0)
                    {
                        results = results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                    }
                    return new PagingResult<ServiceResult>(results, total, currentPage, pageSize);
                }
                else
                {
                    return new PagingResult<ServiceResult>(new List<ServiceResult>(), 0, currentPage, pageSize);
                }

            }
            catch (Exception ex)
            {
                return Error.NotFound("Service not found!");
            }
        }
        }
}
