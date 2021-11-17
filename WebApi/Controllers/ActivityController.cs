using Application.CommandQuery.Commands.Activity;
using Application.CommandQuery.Query.Activity;
using Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ActivityController : BaseController
    {

        [HttpPost("addActivity")]
        public async Task<ActionResult<Unit>> addActivity(ActivityCommand data)
        {
            return await Mediator.Send(data);
        }
        [HttpGet("getActivity")]
        public async Task<ActionResult<List<ActivityResponseDto>>> GetActivity()
        {
            return await Mediator.Send(new ActivityQuery());
        }
        [HttpPut("reschedule")]
        public async Task<ActionResult<Unit>> rescheduleActivity(RescheduleCommand data)
        {
            return await Mediator.Send(data);
        }
        [HttpPut("cancelActivity")]
        public async Task<ActionResult<Unit>> cancelActivity(CancelActivityCommand data)
        {
            return await Mediator.Send(data);
        }
        [HttpPut("endActivity")]
        public async Task<ActionResult<Unit>> endActivity(EndActivityCommand data)
        {
            return await Mediator.Send(data);
        }
        [HttpGet("search/{dateStart}/{dateEnd}/{status}")]
        public async Task<ActionResult<List<ActivityResponseDto>>> search(DateTime dateStart, DateTime dateEnd, string status)
        {
            return await Mediator.Send(new SearchActivityQuery{ dateStart = dateStart, dateEnd = dateEnd, status = status});
        }
    }
}
