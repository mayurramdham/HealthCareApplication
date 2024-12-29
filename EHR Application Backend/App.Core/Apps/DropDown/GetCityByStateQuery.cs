using App.Core.Interface;
using Azure;
using Domain.Entity.AuthProcess;
using Domain.Entity.Register;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.DropDown
{
    public class GetCityByStateQuery:IRequest<object>
    {
        public int CityId { get; set; }
    }
    public class GetCityByStateQueryHandler : IRequestHandler<GetCityByStateQuery, object>
    {
        private readonly IAppDbContext _appDbContext;
        public GetCityByStateQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext= appDbContext;
        }

        public async Task<object> Handle(GetCityByStateQuery request, CancellationToken cancellationToken)
        {
            var cityId = request.CityId;
            var City=await _appDbContext.Set<Domain.Entity.AuthProcess.City>().
                            FirstOrDefaultAsync(s=>s.StateId==request.CityId);
            if (City == null)
            {
                return new
                {
                    StatusCode =404,
                    Message = $"Item with ID {cityId} not found."
                };
            }
            var cities = await _appDbContext.Set<City>()
                     .Where(s => s.StateId == request.CityId)
                     .Select(s => new City
                     {
                         CityId = s.CityId,
                         CityName = s.CityName
                     })
                     .ToListAsync(cancellationToken);

            var response = new
            {
                status = 200,
                message = "all city data",
                cityresponse = cities
            };
            return response;
        }
    }
}
