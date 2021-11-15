using Application.CommandQuery.Commands;
using Application.CommandQuery.Commands.Property;
using Application.CommandQuery.Query;
using Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class PropertyController : BaseController
    {
        [HttpPost("addProperty")]
        public async Task<ActionResult<Unit>> addProperty(PropertyCommand data)
        {
            return await Mediator.Send(data);
        }

        [HttpGet("getProperties")]
        public async Task<ActionResult<List<PropertyDto>>> GetProperty()
        {
            return await Mediator.Send(new PropertyQuery());
        }
        [HttpPut("updateProperty")]
        public async Task<ActionResult<Unit>> addProperty(PropertyCommandUp data)
        {
            return await Mediator.Send(data);
        }
        [HttpDelete("deleteProperty/{id}")]
        public async Task<ActionResult<Unit>> deleteProperty(int id)
        {
            return await Mediator.Send(new PropertyCommandDel { id = id});
        }
    }
}
