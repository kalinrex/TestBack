using Application.ErrorHandler;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CommandQuery.Commands
{
    public class PropertyCommand : IRequest
    {
        public string title { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public string status { get; set; }
    }
    public class PropertyCommandaHandler : IRequestHandler<PropertyCommand>
    {
        private readonly ActivityDbContext _context;
        public PropertyCommandaHandler(ActivityDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PropertyCommand request, CancellationToken cancellationToken)
        {
            var command = await _context.Property.Where(x => x.title == request.title).FirstOrDefaultAsync();
            if(command != null)
                throw new ExceptionHandler(HttpStatusCode.BadRequest, new {Errors = "El registro ya existe"});
            var property = new Domain.Entities.Property
            {
                title = request.title,
                address = request.address,
                description = request.description,
                status = request.status,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
            };
            _context.Add(property);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
                return Unit.Value;

            throw new Exception("Hubo un error");
        }
    }
}
