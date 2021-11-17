using Application.Constant;
using Application.ErrorHandler;
using Application.Utils;
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
    public class RescheduleCommand: IRequest
    {
        public int activity_id { get; set; }
        public DateTime reschedule { get; set; }
    }
    public class RescheduleCommandHandler : IRequestHandler<RescheduleCommand>
    {
        private readonly ActivityDbContext _context;
        private readonly Helpers _helpers;

        public RescheduleCommandHandler(ActivityDbContext context, Helpers helpers)
        {
            _context = context;
            _helpers = helpers;
        }
        public async Task<Unit> Handle(RescheduleCommand request, CancellationToken cancellationToken)
        {
            var command = await _context.Activity.Where(x => x.id == request.activity_id).FirstOrDefaultAsync();
            if (command == null)
                throw new ExceptionHandler(System.Net.HttpStatusCode.NotFound, new {Errors = "No se encontro el registro"});

            int compareDates = _helpers.todayValidate(request.reschedule);
            if (compareDates > 0) 
                throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "La fecha de la actividad no puede ser menor a la fecha actual" });

            bool same = await _helpers.sameDateSchedule(command.property_id, request.reschedule, request.activity_id);
            if(same)
                throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "La fecha y hora seleccionada ya estan registradas en otra actividad" });
            if(command.status == StatusConst.Done)
                throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "No se puede reagendar una actividad finalizada" });
            if(command.status == StatusConst.Cancel)
                throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "No se puede reagendar una actividad cancelada" });
            if (command.schedule == request.reschedule)
                throw new ExceptionHandler(System.Net.HttpStatusCode.BadRequest, new { Errors = "La fecha y hora seleccionada ya esta registrada en esta actividad" });

            command.schedule = request.reschedule;
            command.updated_at = DateTime.Now;
            var response = await _context.SaveChangesAsync();
            if (response > 0)
                return Unit.Value;
            throw new Exception().InnerException;
        }
    }
}
