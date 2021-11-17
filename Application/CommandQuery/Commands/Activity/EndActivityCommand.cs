using Application.Constant;
using Application.ErrorHandler;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CommandQuery.Commands.Activity
{
    public class EndActivityCommand : IRequest
    {
        public int activity_id { get; set; }
    }
    public class EndActivityCommandHandler : IRequestHandler<EndActivityCommand>
    {
        private readonly ActivityDbContext _context;

        public EndActivityCommandHandler(ActivityDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(EndActivityCommand request, CancellationToken cancellationToken)
        {
            var query = await  _context.Activity.FirstOrDefaultAsync(x => x.id == request.activity_id);
            if (query == null)
                throw new ExceptionHandler(System.Net.HttpStatusCode.NotFound, new { Errors = "No se encontro el registro" });

            if (query.status == StatusConst.Active)
                query.status = StatusConst.Done;
            else
                throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "La actividad no cuenta con un status activo" });

            query.updated_at = DateTime.Now;
            var response = await _context.SaveChangesAsync();
            if (response > 0)
                return Unit.Value;

            throw new Exception("No se guardaron los cambios");
        }
    }
}
