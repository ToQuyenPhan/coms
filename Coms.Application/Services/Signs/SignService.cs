using Coms.Application.Common.Intefaces.Persistence;
using ErrorOr;

namespace Coms.Application.Services.Signs
{
    public class SignService : ISignService
    {
        private readonly IContractFileRepository _contractFileRepository;
        private readonly IContractAnnexFileRepository _contractAnnexFileRepository;
        private readonly ILiquidationRecordFileRepository _liquidationRecordFileRepository;
        public SignService(IContractFileRepository contractFileRepository,
            IContractAnnexFileRepository contractAnnexFileRepository,
            ILiquidationRecordFileRepository liquidationRecordFileRepository)
        {
            _contractFileRepository = contractFileRepository;
            _contractAnnexFileRepository = contractAnnexFileRepository;
            _liquidationRecordFileRepository = liquidationRecordFileRepository;
        }
        public async Task<ErrorOr<ResponseModel>> UploadVersion(Guid fileId, byte[] document)
        {
            try
            {
                var contractFile = await _contractFileRepository.GetContractFileById(fileId);
                if (contractFile is not null)
                {
                    contractFile.FileData = document;
                    contractFile.UpdatedDate = DateTime.Now;
                    await _contractFileRepository.Update(contractFile);
                    return new ResponseModel()
                    {
                        isSuccess = true,
                        code = 200,
                        responseSuccess = Convert.ToBase64String(contractFile.FileData),
                        responseFailed = null
                    };
                }
                var annexFile = await _contractAnnexFileRepository.GetContractAnnexFileById(fileId);
                if (annexFile != null)
                {
                    annexFile.FileData = document;
                    annexFile.UpdatedDate = DateTime.Now;
                    await _contractAnnexFileRepository.Update(annexFile);
                    return new ResponseModel()
                    {
                        isSuccess = true,
                        code = 200,
                        responseSuccess = Convert.ToBase64String(annexFile.FileData),
                        responseFailed = null
                    };
                }
                var liquidationFile = await _liquidationRecordFileRepository.GetLiquidationRecordFileById(fileId);
                if (liquidationFile != null)
                {
                    liquidationFile.FileData = document;
                    liquidationFile.UpdatedDate = DateTime.Now;
                    await _liquidationRecordFileRepository.Update(liquidationFile);
                    return new ResponseModel()
                    {
                        isSuccess = true,
                        code = 200,
                        responseSuccess = Convert.ToBase64String(liquidationFile.FileData),
                        responseFailed = null

                    };
                }
                return new ResponseModel()
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
