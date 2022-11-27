using Application.Interfaces.Repositories;
using AutoMapper;
using Common.Infrastructure;
using Common.Infrastructure.Exceptions;
using Common.Models.Queries;
using Common.Models.RequestModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commends.User
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            Api.Core.Domain.Models.User dbUser = await _userRepository.GetSingleAsync(x => x.EmailAddress == request.EmailAdress);

            if (dbUser == null)
                throw new DatabaseValidationException("User not found!");

            string pass = PasswordEncryptor.Encrpt(request.Password);
            if (dbUser.Password != pass)
                throw new DatabaseValidationException("Password is wrong!");

            if (!dbUser.EmailConfirmed)
                throw new DatabaseValidationException("Email adress is not confirmed yet!");

            LoginUserViewModel result = _mapper.Map<LoginUserViewModel>(dbUser);

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
                new Claim(ClaimTypes.Email, dbUser.EmailAddress),
                new Claim(ClaimTypes.Name, dbUser.UserName),
                new Claim(ClaimTypes.GivenName, dbUser.FirstName),
                new Claim(ClaimTypes.Surname, dbUser.LastName)
            };


            result.Token = GenerateToken(claims);

            return result;
        }
        private string GenerateToken(Claim[] claims)
        {
            SymmetricSecurityKey key = new (Encoding.UTF8.GetBytes(_configuration["AuthConfig:Secret"]));
            SigningCredentials creds = new (key, SecurityAlgorithms.HmacSha256);
            DateTime expiry = DateTime.Now.AddDays(10);

            JwtSecurityToken token = new (claims: claims,
                                             expires: expiry,
                                             signingCredentials: creds,
                                             notBefore: DateTime.Now);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
