using Application.Dtos;
using Application.ErrorHandler;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth
{
    public class AuthCommand : IRequest<UserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AuthCommandHandler : IRequestHandler<AuthCommand, UserDto>
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IJwtGenerator jwtGenerator;

        public AuthCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator jwtGenerator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtGenerator = jwtGenerator;
        }
        public async Task<UserDto> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            var user = await this.userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ExceptionHandler(HttpStatusCode.BadRequest, new { Errors = "Email incorrecto." });
            }
            var result = await this.signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded)
            {
                var token = this.jwtGenerator.CreateToken(user);
                return new UserDto
                {
                    NombreCompleto = user.fullName,
                    Token = token.Token,
                    Expiration = token.Expiration,
                    Username = user.UserName,
                    Email = user.Email
                };
            }
            throw new ExceptionHandler(HttpStatusCode.BadRequest, new { Errors = "Email o Password incorrecto." });
        }
    }
}
