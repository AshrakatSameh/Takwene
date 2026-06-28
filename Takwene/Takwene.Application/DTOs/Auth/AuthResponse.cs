using System;

namespace Takwene.Application.DTOs.Auth
{
    public record AuthResponse(string Token, DateTime ExpiresAt);
}
