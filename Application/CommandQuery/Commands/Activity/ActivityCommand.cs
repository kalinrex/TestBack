using Application.Constant;
using Application.ErrorHandler;
using Application.Utils;
using Domain.Entities;
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

namespace Application.CommandQuery.Commands.Activity
{
    #region ++Command++
    public class ActivityCommand : IRequest
    {
        public int property_id { get; set; }
        public string title { get; set; }
        public DateTime schedule { get; set; }
    }
    #endregion

    #region ++Validation++
    public class ActivityCommandValidation : AbstractValidator<ActivityCommand>
    {
        public ActivityCommandValidation()
        {
            RuleFor(x => x.property_id).NotNull().WithMessage("El campo es requerido");
            RuleFor(x => x.title).NotEmpty().WithMessage("El Campo es requerido");
            RuleFor(x => x.schedule).NotNull().WithMessage("El Campo es requerido");
        }
    }
    #endregion

    #region ++CommandHandler++
    public class ActivityCommandHandler : IRequestHandler<ActivityCommand>
    {
        private readonly ActivityDbContext _context;
        private readonly Helpers _helpers;

        public ActivityCommandHandler(ActivityDbContext context, Helpers helpers)
        {
            _context = context;
            _helpers = helpers;
        }
        public async Task<Unit> Handle(ActivityCommand request, CancellationToken cancellationToken)
        {
            int compareDate = _helpers.todayValidate(request.schedule);
            if (compareDate > 0) //si today es mayor a la fecha schedule no se registra actividad
                throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "La fecha de la actividad no puede ser menor a la fecha actual" });
            
            var propertyDisabled = await _context.Property.FirstOrDefaultAsync(x => x.id == request.property_id);
            if(propertyDisabled != null)
            {
                if (propertyDisabled.status == StatusConst.Disabled)
                    throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "La propiedad esta deshabilitada, seleccione otra propiedad o modifique el estatus de la propiedad a Active" });
            }

            bool sameDate = await _helpers.sameDateSchedule(request.property_id, request.schedule);
            if (sameDate)
                throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "La fecha y hora seleccionada ya estan registradas en otra actividad" });

            var existProperty = await _context.Property.FirstOrDefaultAsync(x => x.id == request.property_id);
            if(existProperty == null)
                throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "La propiedad seleccionada no existe" });


            var act = new Domain.Entities.Activity();
            act.property_id = request.property_id;
            act.schedule = request.schedule;
            act.title = request.title;
            act.created_at = DateTime.Now;
            act.updated_at = DateTime.Now;
            act.status = StatusConst.Active;
            var survay = new Survey();
            survay.answers = @"{""P1"": ""Respuesta pregunta1"", ""P2"": ""Respuesta pregunta2"", ""P2"": ""Respuesta pregunta2""}"; 
            survay.created_at = DateTime.Now;
            act.Survey = survay;

            _context.Add(act);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
                return Unit.Value;

            throw new Exception("Hubo un error");
        }
    }

    #endregion
}
