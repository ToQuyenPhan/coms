﻿using ErrorOr;

namespace Coms.Domain.Common.Errors
{
    public static class Errors
    {
        public static class User
        {
            public static Error IncorrectUsername => Error.Validation(code: "User.IncorrectUsername",
                description: "Your username is incorrect");

            public static Error IncorrectPassword => Error.Validation(code: "User.IncorrectPassword",
                description: "Your password is incorrect");
        }
    }
}
