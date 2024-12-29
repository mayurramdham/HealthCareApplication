using App.Core.Interface;
using Domain.Entity.Register;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.DropDown
{
    public class GetStatesByCountryQuery : IRequest<object>
    {
        public int CountryId { get; set; }

        public GetStatesByCountryQuery(int countryId)
        {
            CountryId = countryId;
        }
        public class GetStatesByCountryQueryHandler : IRequestHandler<GetStatesByCountryQuery, object>
        {
            private readonly IAppDbContext _appDbContext;

            public GetStatesByCountryQueryHandler(IAppDbContext appDbContext)
            {
                _appDbContext = appDbContext;
            }

            public async Task<object> Handle(GetStatesByCountryQuery request, CancellationToken cancellationToken)
            {
                var states = await _appDbContext.Set<State>()
                    .Where(s => s.CountryId == request.CountryId)
                    .Select(s => new State
                    {
                        StateId = s.StateId,
                        StateName = s.StateName
                    })
                    .ToListAsync(cancellationToken);

                var response = new
                {
                    status = 200,
                    message = "All State Data by countryId",
                    state = states

                };
                return response;
            }
        }
    }
}
