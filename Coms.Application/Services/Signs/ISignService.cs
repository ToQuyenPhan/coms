using Coms.Application.Services.ContractFiles;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Signs
{
    public interface ISignService
    {
        Task<ErrorOr<ResponseModel>> UploadVersion(Guid fileId, byte[] file);
    }
}
