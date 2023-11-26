using Coms.Domain.Entities;

namespace Coms.Application.Common.Intefaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
        string GeneratePartnerToken(Partner partner);
    }
}
