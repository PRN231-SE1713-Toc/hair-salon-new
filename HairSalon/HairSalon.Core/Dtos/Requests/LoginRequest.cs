﻿namespace HairSalon.Core.Dtos.Requests
{
    public class LoginRequest
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
