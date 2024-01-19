using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Documents
{
    public class DocumentService : IDocumentService
    {
        private readonly IContractFileRepository _contractFileRepository;
        private readonly IContractAnnexFileRepository _contractAnnexFileRepository;
        private readonly ILiquidationRecordFileRepository _liquidationRecordFileRepository;
        public DocumentService(IContractFileRepository contractFileRepository,
            IContractAnnexFileRepository contractAnnexFileRepository,
            ILiquidationRecordFileRepository liquidationRecordFileRepository)
        {
            _contractFileRepository = contractFileRepository;
            _contractAnnexFileRepository = contractAnnexFileRepository;
            _liquidationRecordFileRepository = liquidationRecordFileRepository;
        }

        //public async Task<ErrorOr<ResponseModel>> DownloadDocument(Guid fileId, Guid versionId)
        //{
        //    try
        //    {
        //        if (_contractFileRepository.GetContractFileById(fileId).Result is not null)
        //        {
        //            var contracFile = _contractFileRepository.GetContractFileById(fileId).Result;
        //            if (contracFile != null)
        //            {
        //                return new ResponseModel
        //                {
        //                    isSuccess = true,
        //                    code = 200,
        //                    responseSuccess = Convert.ToBase64String(contracFile.FileData),
        //                    responseFailed = null,
        //                };
        //            }
        //        }
        //        else if (_contractAnnexFileRepository.GetContractAnnexFileById(fileId).Result is not null)
        //        {
        //            var annexFile = _contractAnnexFileRepository.GetContractAnnexFileById(fileId).Result;
        //            if (annexFile != null)
        //            {
        //                return new ResponseModel
        //                {
        //                    isSuccess = true,
        //                    code = 200,
        //                    responseSuccess = Convert.ToBase64String(annexFile.FileData),
        //                    responseFailed = null,
        //                };
        //            }
        //        }
        //        else if (_liquidationRecordFileRepository.GetLiquidationRecordFileById(fileId).Result is not null)
        //        {
        //            var liquidationFile = _liquidationRecordFileRepository.GetLiquidationRecordFileById(fileId).Result;
        //            if (liquidationFile != null)
        //            {
        //                return new ResponseModel
        //                {
        //                    isSuccess = true,
        //                    code = 200,
        //                    responseSuccess = Convert.ToBase64String(liquidationFile.FileData),
        //                    responseFailed = null,
        //                };
        //            }
        //        }
        //        else
        //        {
        //            return new ResponseModel
        //            {
        //                isSuccess = false,
        //                code = 404,
        //                responseSuccess = null,
        //                responseFailed = "ContractFile not found!",
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error.Failure("500", ex.Message);
        //    }
        //}

        public async Task<ErrorOr<ResponseModel>> DownloadDocument(Guid fileId, Guid versionId)
        {
            try
            {
                var contractFile = await _contractFileRepository.GetContractFileById(fileId);
                if (contractFile != null)
                {
                    return new ResponseModel()
                    {
                        isSuccess = true,
                        code = 200,
                        responseSuccess = Convert.ToBase64String(contractFile.FileData),
                        responseFailed = null,
                    };
                }

                var annexFile = await _contractAnnexFileRepository.GetContractAnnexFileById(fileId);
                if (annexFile != null)
                {
                    return new ResponseModel()
                    {
                        isSuccess = true,
                        code = 200,
                        responseSuccess = Convert.ToBase64String(annexFile.FileData),
                        responseFailed = null,
                    };
                }

                /*var liquidationFile = await _liquidationRecordFileRepository.GetLiquidationRecordFileById(fileId);
                if (liquidationFile != null)
                {
                    return new ResponseModel()
                    {
                        isSuccess = true,
                        code = 200,
                        responseSuccess = Convert.ToBase64String(liquidationFile.FileData),
                        responseFailed = null,
                    };
                }*/

                return new ResponseModel
                {
                    isSuccess = false,
                    code = 404,
                    responseSuccess = null,
                    responseFailed = "File not found!",
                };
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

    }
}
