using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using R5T.Corcyra;
using R5T.Endalia;
using R5T.Venetia;


namespace R5T.Sundsvall.Database
{
    public class EndpointRepository : DatabaseRepositoryBase<EndpointDbContext>, IEndpointRepository
    {
        public EndpointRepository(DbContextOptions<EndpointDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public override EndpointDbContext GetNewDbContext()
        {
            var dbContext = new EndpointDbContext(this.DbContextOptions);
            return dbContext;
        }

        public EndpointIdentity GetEndpointIdentityForCatchment(CatchmentIdentity catchment)
        {
            throw new NotImplementedException();
        }

        public EndpointTypeIdentity GetEndpointType(EndpointIdentity endpoint)
        {
            throw new NotImplementedException();
        }

        public EndpointIdentity New()
        {
            throw new NotImplementedException();
        }

        public void SetEndpointIdentityForCatchment(CatchmentIdentity catchment, EndpointIdentity endpoint)
        {
            throw new NotImplementedException();
        }

        public void SetEndpointType(EndpointIdentity endpoint, EndpointTypeIdentity endpointType)
        {
            throw new NotImplementedException();
        }

        public EndpointInfo GetInfo(EndpointIdentity identity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EndpointInfo> GetAllInfos()
        {
            throw new NotImplementedException();
        }
    }
}
