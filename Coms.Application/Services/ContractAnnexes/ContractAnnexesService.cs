using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using LinqKit;
using System.Linq;

namespace Coms.Application.Services.ContractAnnexes
{
    public class ContractAnnexesService : IContractAnnexesService
    {
        private readonly IContractAnnexRepository _contractAnnexRepository;

        public ContractAnnexesService(IContractAnnexRepository contractAnnexRepository)
        {
            _contractAnnexRepository = contractAnnexRepository;
        }
        //get all contractannexes
        public async Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetContractAnnexes(string name, int? status, int currentPage, int pageSize)
        {
            var predicate = PredicateBuilder.New<ContractAnnex>(true);
            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(c => c.ContractAnnexName.Contains(name.Trim()));
            }
            if (status is not null)
            {
                if (status >= 0)
                {
                    predicate = predicate.And(c => c.Status.Equals((DocumentStatus)status));
                }
            }
            var contractAnnexes = await _contractAnnexRepository.GetContractAnnexes();
            IList<ContractAnnex> filteredList = contractAnnexes.Where(predicate).ToList();
            var total = filteredList.Count();
            if (currentPage > 0 && pageSize > 0)
            {
                filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            IList<ContractAnnexesResult> responses = new List<ContractAnnexesResult>();
            foreach (var contractAnnex in filteredList)
            {
                var contractAnnexResult = new ContractAnnexesResult()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    UpdatedDate = contractAnnex.UpdatedDate,
                    Status = (DocumentStatus)contractAnnex.Status,
                    ContractId = contractAnnex.ContractId,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                responses.Add(contractAnnexResult);
            }
            return new
                PagingResult<ContractAnnexesResult>(responses, total, currentPage, pageSize);
        }

        //get contractannexes by contractId
        public async Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetContractAnnexesByContractId(int contractId, string name, int? status, int currentPage, int pageSize)
        {
            var predicate = PredicateBuilder.New<ContractAnnex>(true);
            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(c => c.ContractAnnexName.Contains(name.Trim()));
            }
            if (status is not null)
            {
                if (status >= 0)
                {
                    predicate = predicate.And(c => c.Status.Equals((DocumentStatus)status));
                }
            }
            var contractAnnexes = await _contractAnnexRepository.GetContractAnnexesByContractId(contractId);
            IList<ContractAnnex> filteredList = contractAnnexes.Where(predicate).ToList();
            var total = filteredList.Count();
            if (currentPage > 0 && pageSize > 0)
            {
                filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            IList<ContractAnnexesResult> responses = new List<ContractAnnexesResult>();
            foreach (var contractAnnex in filteredList)
            {
                var contractAnnexResult = new ContractAnnexesResult()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    UpdatedDate = contractAnnex.UpdatedDate,
                    Status = (DocumentStatus)contractAnnex.Status,
                    ContractId = contractAnnex.ContractId,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                responses.Add(contractAnnexResult);
            }
            return new
                PagingResult<ContractAnnexesResult>(responses, total, currentPage, pageSize);
        }
        //get contractannexes by contractAnnexId
        public async Task<ErrorOr<ContractAnnexesResult>> GetContractAnnexesById(int id)
        {
            var contractAnnex = await _contractAnnexRepository.GetContractAnnexesById(id);
            if (contractAnnex is not null)
            {
                var contractAnnexResult = new ContractAnnexesResult()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    UpdatedDate = contractAnnex.UpdatedDate,
                    Status = (DocumentStatus)contractAnnex.Status,
                    ContractId = contractAnnex.ContractId,
                    Code = contractAnnex.Code,
                    Link = contractAnnex.Link
                };
                return contractAnnexResult;
            }
            else
            {
                return Error.NotFound("404", "ContractAnnex is not found!");
            }
        }
    }
}
