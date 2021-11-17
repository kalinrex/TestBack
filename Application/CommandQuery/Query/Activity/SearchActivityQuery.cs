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
    public class SearchActivityQuery: IRequest<List<ActivityResponseDto>>
    {
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public string status { get; set; }
    }
    public class SearchActivityQueryHandler : IRequestHandler<SearchActivityQuery, List<ActivityResponseDto>>
    {
        private readonly ActivityDbContext _context;
        private readonly IMapper _mapper;
        public SearchActivityQueryHandler(ActivityDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ActivityResponseDto>> Handle(SearchActivityQuery request, CancellationToken cancellationToken)
        {
            var query = await _context.Activity.Where(x => x.schedule >= request.dateStart && x.schedule <= request.dateEnd && (request.status == "All" || x.status == request.status)).Include(prop => prop.Property).Include(sub => sub.Survey).ToListAsync();
            var response = _mapper.Map<List<Domain.Entities.Activity>, List<ActivityResponseDto>>(query);
            return response;
        }
    }
}
