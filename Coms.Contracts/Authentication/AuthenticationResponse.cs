namespace Coms.Contracts.Authentication
{
    public record AuthenticationResponse
    (
        int Id,
        string Username,
        string Token
    );
}
