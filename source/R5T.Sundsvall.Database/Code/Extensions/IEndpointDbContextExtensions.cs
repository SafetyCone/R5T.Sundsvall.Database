using System;
using System.Linq;

using R5T.Endalia;

using EndpointEntity = R5T.Sundsvall.Database.Entities.Endpoint;
using EndpointTypeEntity = R5T.Sundsvall.Database.Entities.EndpointType;


namespace R5T.Sundsvall.Database
{
    public static class IEndpointDbContextExtensions
    {
        public static IQueryable<EndpointEntity> GetEndpoint(this IEndpointDbContext dbContext, EndpointIdentity endpointIdentity)
        {
            var endpointQueryable = dbContext.Endpoints.Where(x => x.GUID == endpointIdentity.Value);
            return endpointQueryable;
        }

        public static IQueryable<EndpointTypeEntity> GetEndpointType(this IEndpointDbContext dbContext, EndpointTypeIdentity endpointTypeIdentity)
        {
            var endpointTypeQueryable = dbContext.EndpointTypes.Where(x => x.GUID == endpointTypeIdentity.Value);
            return endpointTypeQueryable;
        }
    }
}
