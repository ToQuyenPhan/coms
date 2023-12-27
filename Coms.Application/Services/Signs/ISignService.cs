using ErrorOr;

namespace Coms.Application.Services.Signs
{
    public interface ISignService
    {
        Task<ErrorOr<ResponseModel>> UploadVersion(Guid fileId, byte[] file);
    }
}
