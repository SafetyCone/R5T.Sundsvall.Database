using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using R5T.Endalia;
using R5T.Venetia;


namespace R5T.Sundsvall.Database
{
    public class EndpointAndTypeRepository<TDbContext> : ProvidedDatabaseRepositoryBase<TDbContext>, IEndpointAndTypeRepository
        where TDbContext : DbContext, IEndpointDbContext
    {
        public EndpointAndTypeRepository(DbContextOptions<TDbContext> dbContextOptions, IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextOptions, dbContextProvider)
        {
        }

        public async Task<Dictionary<string, List<EndpointIdentity>>> GetEndpointIdentitiesByEndpointTypeName(IEnumerable<EndpointIdentity> endpointIdentities)
        {
            var endpointIdentityValues = endpointIdentities.Select(x => x.Value).ToList();

            var endpointIdentitiesByEndpointTypeName = await this.ExecuteInContextAsync(async dbContext =>
            {

                // var old =
                //     from endpoint in dbContext.Endpoints
                //     join endpointType in dbContext.EndpointTypes on endpoint.EndpointTypeID equals endpointType.ID
                //     where endpointIdentityValues.Contains(endpoint.GUID)
                //     group new { endpointType.Name, endpoint.GUID } by endpointType.Name into endpointsByEndpointTypeNameGroup
                //     select endpointsByEndpointTypeNameGroup;

                var gettingEndpoints =
                    from endpoint in dbContext.Endpoints
                    join endpointType in dbContext.EndpointTypes on endpoint.EndpointTypeID equals endpointType.ID
                    where endpointIdentityValues.Contains(endpoint.GUID)
                    select new { endpoint, endpointType };

                var endpoints = await gettingEndpoints.ToListAsync();

                var gettingEndpointsByEndpointTypeName =
                    from entry in endpoints
                    group new { entry.endpointType.Name, entry.endpoint.GUID } by entry.endpointType.Name into endpointsByEndpointTypeNameGroup
                    select endpointsByEndpointTypeNameGroup;


                var output = gettingEndpointsByEndpointTypeName.ToDictionary(
                    grouping => grouping.Key,
                    grouping => grouping
                        .Select(y => EndpointIdentity.From(y.GUID))
                        .ToList());

                return output;
            });

            return endpointIdentitiesByEndpointTypeName;
        }
    }
}
