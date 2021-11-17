using Application.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CommandQuery.Query.Activity
{
    public class ActivityQuery: IRequest<List<ActivityResponseDto>>
    {
    }
    public class ActivityQueryHandler : IRequestHandler<ActivityQuery, List<ActivityResponseDto>>
    {
        private readonly ActivityDbContext _context;
        private readonly IMapper _mapper;

        public ActivityQueryHandler(ActivityDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ActivityResponseDto>> Handle(ActivityQuery request, CancellationToken cancellationToken)
        {
            DateTime dateStart = DateTime.Now.AddDays(-3);
            DateTime dateEnd = DateTime.Now.AddDays(14);

            var query = await _context.Activity.Where(ac => dateStart <= ac.schedule && ac.schedule <= dateEnd).Include(prop => prop.Property).Include(sub => sub.Survey).ToListAsync();
            var response = _mapper.Map<List<Domain.Entities.Activity>, List<ActivityResponseDto>>(query);
            return response;
        }
    }
}
