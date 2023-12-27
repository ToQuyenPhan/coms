using Coms.Application.Services.ContractFiles;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Documents
{
    public interface IDocumentService
    {
        Task<ErrorOr<ResponseModel>> DownloadDocument(Guid fileId, Guid versionId);
    }
}
