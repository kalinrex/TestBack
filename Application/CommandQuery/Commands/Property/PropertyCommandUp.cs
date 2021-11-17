using Application.Constant;
using Application.ErrorHandler;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CommandQuery.Commands
{
    #region ++Command++
    public class PropertyCommandUp : IRequest
    {
        public int id { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string description { get; set; }
        public string status { get; set; }
    }

    #endregion

    #region ++Validation++
    public class PropertyCommandUpValidation : AbstractValidator<PropertyCommandUp>
    {
        public PropertyCommandUpValidation()
        {
            RuleFor(x => x.id).NotNull().WithMessage("El campo es requerido");
            RuleFor(x => x.title).NotEmpty().WithMessage("El campo es requerido");
            RuleFor(x => x.address).NotEmpty().WithMessage("El campo es requerido");
            RuleFor(x => x.description).NotEmpty().WithMessage("El campo es requerido");
            RuleFor(x => x.status).NotEmpty().WithMessage("El campo es requerido");
        }
    }
    #endregion

    #region ++CommandHandler
    public class PropertyCommandHandlerUp : IRequestHandler<PropertyCommandUp>
    {
        private readonly ActivityDbContext _context;

        public PropertyCommandHandlerUp(ActivityDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PropertyCommandUp request, CancellationToken cancellationToken)
        {
            var command = await _context.Property.FirstOrDefaultAsync(x => x.title == request.title);
            var exist = await _context.Property.FirstOrDefaultAsync(x => x.title == request.title && x.id != request.id);
            if (command == null) 
                throw new ExceptionHandler(System.Net.HttpStatusCode.NotFound, new {Errors = "No se encontro el registro"});
            if(exist != null)
                throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "El registro ya existe" });

            command.title = request.title ?? command.title;
            command.address = request.address ?? command.address;
            command.description = request.description ?? command.description;
            command.updated_at = DateTime.Now;
            command.status = request.status ?? command.status;

            if (request.status == StatusConst.Disabled)
                command.disabled_at = DateTime.Now;
            else
                command.disabled_at = null;

            var response = await _context.SaveChangesAsync();
            if (response > 0)
                return Unit.Value;

            throw new Exception("Hubo un error");
        }
    }

    #endregion
}
