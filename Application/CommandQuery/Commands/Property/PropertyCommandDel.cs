using Application.ErrorHandler;
using FluentValidation;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CommandQuery.Commands.Property
{
    #region ++Command++
    public class PropertyCommandDel : IRequest
    {
        public int id { get; set; }
    }
    #endregion

    #region ++Validation++
    public class PropertyCommandDelValidation : AbstractValidator<PropertyCommandDel>
    {
        public PropertyCommandDelValidation()
        {
            RuleFor(x => x.id).NotNull().WithMessage("El campo es requerido");
        }
    }
    #endregion

    #region ++CommandHandler++
    public class PropertyCommandDelHandler : IRequestHandler<PropertyCommandDel>
    {
        private readonly ActivityDbContext _context;

        public PropertyCommandDelHandler(ActivityDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(PropertyCommandDel request, CancellationToken cancellationToken)
        {
            var command = await _context.Property.FindAsync(request.id);
            if (command == null)
                throw new ExceptionHandler(HttpStatusCode.NotFound, new { Errors = "No se encontro el registro" });

            _context.Remove(command);

            var response = await _context.SaveChangesAsync();

            if (response > 0)
                return Unit.Value;

            throw new Exception("Hubo un error");

        }
    }

    #endregion
}
