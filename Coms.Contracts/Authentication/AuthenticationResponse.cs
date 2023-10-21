namespace Coms.Contracts.Authentication
{
    public record AuthenticationResponse
    (
        Guid Id,
        string Username,
        string Token
    );
}
