using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using R5T.Corcyra;
using R5T.Endalia;
using R5T.Venetia;

using EndpointEntity = R5T.Sundsvall.Database.Entities.Endpoint;


namespace R5T.Sundsvall.Database
{
    public class EndpointRepository<TDbContext> : ProvidedDatabaseRepositoryBase<TDbContext>, IEndpointRepository
        where TDbContext: DbContext, IEndpointDbContext
    {
        public EndpointRepository(DbContextOptions<TDbContext> dbContextOptions, IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextOptions, dbContextProvider)
        {
        }

        public EndpointIdentity GetEndpointForCatchment(CatchmentIdentity catchment)
        {
            var endpointIdentity = this.ExecuteInContext(dbContext =>
            {
                var endpointGuid = dbContext.EndpointToCatchmentMappings.Where(x => x.CatchmentGUID == catchment.Value).Select(x => x.EndpointGUID).Single();

                var output = EndpointIdentity.From(endpointGuid);
                return output;
            });

            return endpointIdentity;
        }

        public EndpointTypeIdentity GetEndpointType(EndpointIdentity endpoint)
        {
            var endpointTypeIdentity = this.ExecuteInContext(dbContext =>
            {
                var endpointTypeGuid = dbContext.Endpoints.Where(x => x.GUID == endpoint.Value).Select(x => x.EndpointType.GUID).Single();

                var output = EndpointTypeIdentity.From(endpointTypeGuid);
                return output;
            });

            return endpointTypeIdentity;
        }

        public EndpointIdentity New()
        {
            var endpointIdentity = EndpointIdentity.New();

            this.ExecuteInContext(dbContext =>
            {
                var entity = new EndpointEntity()
                {
                    GUID = endpointIdentity.Value,
                };

                dbContext.Endpoints.Add(entity);

                dbContext.SaveChanges();
            });

            return endpointIdentity;
        }

        public EndpointIdentity AddByName(EndpointInfo endpointInfo)
        {
            var endpointIdentity = EndpointIdentity.New();

            this.ExecuteInContext(dbContext =>
            {
                var endpointTypeID = dbContext.EndpointTypes.Where(x => x.Name == endpointInfo.TypeInfo.Name).Select(x => x.ID).Single();

                var endpointEntity = new Entities.Endpoint()
                {
                    GUID = endpointIdentity.Value,
                    Name = endpointInfo.Name,
                    EndpointTypeID = endpointTypeID,
                };

                dbContext.Endpoints.Add(endpointEntity);

                dbContext.SaveChanges();
            });

            return endpointIdentity;
        }

        public void SetEndpointForCatchment(CatchmentIdentity catchment, EndpointIdentity endpoint)
        {
            this.ExecuteInContext(dbContext =>
            {
                var mapping = dbContext.EndpointToCatchmentMappings.Where(x => x.EndpointGUID == endpoint.Value).SingleOrDefault();

                if(mapping is object)
                {
                    mapping.CatchmentGUID = catchment.Value;
                }
                else
                {
                    mapping = new Entities.EndpointToCatchmentMapping()
                    {
                        EndpointGUID = endpoint.Value,
                        CatchmentGUID = catchment.Value,
                    };

                    dbContext.Add(mapping);
                }

                dbContext.SaveChanges();
            });
        }

        public void SetEndpointType(EndpointIdentity endpoint, EndpointTypeIdentity endpointType)
        {
            this.ExecuteInContext(dbContext =>
            {
                var endpointTypeID = dbContext.EndpointTypes.AsNoTracking().Where(x => x.GUID == endpointType.Value).Select(x => x.ID).Single();

                var endpointEntity = dbContext.Endpoints.Where(x => x.GUID == endpoint.Value).Single();

                endpointEntity.EndpointTypeID = endpointTypeID;

                dbContext.SaveChanges();
            });
        }

        public EndpointInfo GetInfo(EndpointIdentity identity)
        {
            var endpointInfo = this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.Endpoints.Where(x => x.GUID == identity.Value).Include(x => x.EndpointType).Single();

                var output = entity.ToAppType();
                return output;
            });

            return endpointInfo;
        }

        public IEnumerable<EndpointInfo> GetAllInfos()
        {
            var endpointInfos = this.ExecuteInContext(dbContext =>
            {
                var outputs = dbContext.Endpoints.Include(x => x.EndpointType).ToList().Select(x => x.ToAppType());
                return outputs;
            });

            return endpointInfos;
        }
    }
}
