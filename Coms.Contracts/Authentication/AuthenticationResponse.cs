using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.Authentication
{
    public record AuthenticationResponse
    (
        Guid Id,
        string Username,
        string Token
    );
}
