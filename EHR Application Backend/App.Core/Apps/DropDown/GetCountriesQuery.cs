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
    public class GetCountriesQuery : IRequest<object>
    {
    }
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, object>
    {
        private readonly IAppDbContext _appDbContext;

        public GetCountriesQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = await _appDbContext.Set<Country>()
                .Select(c => new Country
                {
                    CountryId = c.CountryId,
                    CountryName = c.CountryName,
                    PhoneCode = c.PhoneCode
                })
                .ToListAsync(cancellationToken);

            var response = new
            {
                status = 200,
                message = "All Country Data",
                country = countries

            };
            return response;
        }
    }
}
