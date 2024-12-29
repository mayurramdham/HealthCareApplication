﻿using App.Core.Interface;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Appointment.Query
{
    public class GetAllPatient : IRequest<object>
    {
        public int UserTypeId { get; set; }
    }
    public class GetAllPatientHandler : IRequestHandler<GetAllPatient, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        public GetAllPatientHandler(IAppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }

        public async Task<object> Handle(GetAllPatient request, CancellationToken cancellationToken)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new SqlConnection(connectionString);
            const string sql = "select * from [User] Where UserTypeId=@UserTypeId";

            // Execute the query and retrieve the results
            var provider = await connection.QueryAsync<Domain.Entity.AuthProcess.User>(sql, new { UserTypeId = request.UserTypeId });

            // Check if the provider is empty
            if (!provider.Any())
            {
                return new
                {
                    message = "Provider not found",
                    status = 404,
                    data = (object)null
                };
            }

            var response = new
            {
                message = "Get Provider By UserTypeId",
                status = 200,
                data = provider
            };
            return response;
        }

    }
}