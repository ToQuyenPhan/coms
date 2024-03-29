﻿using Coms.Application.Common.Intefaces.Authentication;
using Coms.Application.Common.Intefaces.Services;
using Coms.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Coms.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IDateTimeProvider _dateTimeProvider;

        public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
        {
            _dateTimeProvider = dateTimeProvider;
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(User user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), 
                SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim("role", user.Role.RoleName),
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var securityToken = new JwtSecurityToken(issuer: _jwtSettings.Issuer, 
                audience: _jwtSettings.Audience,
                expires: _dateTimeProvider.UtcNow.AddDays(1), 
                claims: claims, signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public string GeneratePartnerToken(Partner partner)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, partner.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, partner.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, partner.Representative),
                new Claim("CompanyName", partner.CompanyName.ToString()),
                new Claim("Position", partner.RepresentativePosition.ToString()),
                new Claim("Email", partner.Email.ToString()),
                new Claim("Id", partner.Id.ToString()),
                new Claim("role", "Partner"),
                new Claim(ClaimTypes.NameIdentifier, partner.Id.ToString())
            };
            var securityToken = new JwtSecurityToken(issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: _dateTimeProvider.UtcNow.AddDays(1),
                claims: claims, signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
