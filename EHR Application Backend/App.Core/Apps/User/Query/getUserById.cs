using App.Core.Interface;
using Domain.Entity.AuthProcess;
using Domain.Entity.Register;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.User.Query
{
    public class getUserById:IRequest<object>
    {
        public int Id { get; set; }
    }
    public class getUserByUserIdHandler : IRequestHandler<getUserById,object>
    {
        private readonly IAppDbContext _appDbContext;
        public getUserByUserIdHandler(IAppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
        }

        public async Task<object> Handle(getUserById request, CancellationToken cancellationToken)
        {
            var userId = request.Id;
            var getUser=await _appDbContext.Set<Domain.Entity.AuthProcess.User>()
                              .FirstOrDefaultAsync(u=>u.Id == userId);
            if (getUser is null)
            {
                var nullresponse = new
                {
                    status = 404,
                    message = "user not exists or userId is wrong"
                };
                return nullresponse;
            }
            var state=await _appDbContext.Set<State>().FirstOrDefaultAsync(s=>s.StateId == getUser.StateId);
            var country=await _appDbContext.Set<Country>().FirstOrDefaultAsync(c=>c.CountryId == getUser.CountryId);
            var specilization = await _appDbContext.Set<Specialisation>().FirstOrDefaultAsync(c => c.Id == getUser.SpecialisationId);
            var stateName = state.StateName;
            var countryName = country.CountryName;
          //  var specilizationName = specilization.SpecialisationName;
            var response = new
            {
                status = 200,
                message = $"all data for userId {userId}",
                data = getUser,
                state = stateName,
                country = countryName,
              //  specilizations= specilizationName
            };
            return response;
        }  
    }
}
