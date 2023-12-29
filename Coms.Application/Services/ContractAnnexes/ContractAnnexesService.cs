﻿using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using LinqKit;

namespace Coms.Application.Services.ContractAnnexes
{
    public class ContractAnnexesService : IContractAnnexesService
    {
        private readonly IContractAnnexRepository _contractAnnexRepository;
        private readonly IContractRepository _contractRepository;

        public ContractAnnexesService(IContractAnnexRepository contractAnnexRepository, IContractRepository contractRepository)
        {
            _contractAnnexRepository = contractAnnexRepository;
            _contractRepository = contractRepository;
        }
        //get all contractannexes
        public async Task<ErrorOr<PagingResult<ContractAnnexesResult>>> GetContractAnnexes(string name, int? status, int currentPage, int pageSize)
        {
            ExpressionStarter<ContractAnnex> predicate = PredicateBuilder.New<ContractAnnex>(true);
            if (!string.IsNullOrEmpty(name))
            {
                string lowerCaseName = name.ToLower().Trim();
                predicate = predicate.And(c => c.ContractAnnexName.ToLower().Contains(lowerCaseName));
            }
            if (status is not null)
            {
                if (status >= 0)
                {
                    predicate = predicate.And(c => c.Status.Equals((DocumentStatus)status));
                }
            }
            IList<ContractAnnex> contractAnnexes = await _contractAnnexRepository.GetContractAnnexes();
            IList<ContractAnnex> filteredList = contractAnnexes.Where(predicate).ToList();
            int total = filteredList.Count();
            if (currentPage > 0 && pageSize > 0)
            {
                filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            IList<ContractAnnexesResult> responses = new List<ContractAnnexesResult>();
            foreach (ContractAnnex contractAnnex in filteredList)
            {
                string contractName = _contractRepository.GetContract((int)contractAnnex.ContractId).Result.ContractName;
                ContractAnnexesResult contractAnnexResult = new()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    CreatedDateString = contractAnnex.CreatedDate.ToString("dd/MM/yyyy"),
                    UpdatedDate = (DateTime)contractAnnex.UpdatedDate,
                    UpdatedDateString = ((DateTime)contractAnnex.UpdatedDate).ToString("dd/MM/yyyy"),
                    Status = contractAnnex.Status,
                    StatusString = contractAnnex.Status.ToString(),
                    ContractId = (int)contractAnnex.ContractId,
                    ContractName = contractName,
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
            ExpressionStarter<ContractAnnex> predicate = PredicateBuilder.New<ContractAnnex>(true);
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
            IList<ContractAnnex> contractAnnexes = await _contractAnnexRepository.GetContractAnnexesByContractId(contractId);
            IList<ContractAnnex> filteredList = contractAnnexes.Where(predicate).ToList();
            int total = filteredList.Count();
            if (currentPage > 0 && pageSize > 0)
            {
                filteredList = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            IList<ContractAnnexesResult> responses = new List<ContractAnnexesResult>();
            foreach (ContractAnnex contractAnnex in filteredList)
            {
                ContractAnnexesResult contractAnnexResult = new()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    UpdatedDate = (DateTime)contractAnnex.UpdatedDate,
                    Status = contractAnnex.Status,
                    ContractId = (int)contractAnnex.ContractId,
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
            ContractAnnex? contractAnnex = await _contractAnnexRepository.GetContractAnnexesById(id);
            if (contractAnnex is not null)
            {
                ContractAnnexesResult contractAnnexResult = new()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    UpdatedDate = (DateTime)contractAnnex.UpdatedDate,
                    Status = contractAnnex.Status,
                    ContractId = (int)contractAnnex.ContractId,
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

        //delete contractannexes by contractAnnexId
        public async Task<ErrorOr<ContractAnnexesResult>> DeleteContractAnnex(int id)
        {
            ContractAnnex? contractAnnex = await _contractAnnexRepository.GetContractAnnexesById(id);
            if (contractAnnex is not null)
            {
                contractAnnex.Status = DocumentStatus.Deleted;
                await _contractAnnexRepository.UpdateContractAnnexes(contractAnnex);
                ContractAnnexesResult contractAnnexResult = new()
                {
                    Id = contractAnnex.Id,
                    ContractAnnexName = contractAnnex.ContractAnnexName,
                    Version = contractAnnex.Version,
                    CreatedDate = contractAnnex.CreatedDate,
                    UpdatedDate = (DateTime)contractAnnex.UpdatedDate,
                    Status = contractAnnex.Status,
                    ContractId = (int)contractAnnex.ContractId,
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