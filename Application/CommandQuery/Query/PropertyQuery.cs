using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CommandQuery.Query
{
    public class PropertyQuery : IRequest<List<PropertyDto>>
    {
    }

    public class PropertyQueryHandler : IRequestHandler<PropertyQuery, List<PropertyDto>>
    {
        private readonly ActivityDbContext _context;
        private readonly IMapper _mapper;

        public PropertyQueryHandler(ActivityDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<PropertyDto>> Handle(PropertyQuery request, CancellationToken cancellationToken)
        {
            var query = await _context.Property.ToListAsync();
            var properties = _mapper.Map<List<Property>, List<PropertyDto>>(query);
            return properties;
        }
    }

}
