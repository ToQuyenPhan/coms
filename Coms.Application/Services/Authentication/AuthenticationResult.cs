using Coms.Domain.Entities;

namespace Coms.Application.Services.Authentication
{
    public record AuthenticationResult
    (
        string Token
    );
}