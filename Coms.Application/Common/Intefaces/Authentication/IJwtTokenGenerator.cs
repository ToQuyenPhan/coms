namespace Coms.Application.Common.Intefaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid id, string username);
    }
}
