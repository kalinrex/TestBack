using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth
{
    public class CurrentSesion : IRequest<User>
    {
    }
    public class CurrentSesionHandler : IRequestHandler<CurrentSesion, User>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly ICurrentUser _currentUser;
        public CurrentSesionHandler(UserManager<User> userManager, IJwtGenerator jwtGenerator, ICurrentUser currentUser)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _currentUser = currentUser;
        }
        public async Task<User> Handle(CurrentSesion request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(_currentUser.getCurrentUserSesion());
            return user;
        }
    }
}
