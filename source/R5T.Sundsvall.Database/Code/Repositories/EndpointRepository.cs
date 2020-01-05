using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<EndpointIdentity> GetEndpointForCatchment(CatchmentIdentity catchment)
        {
            var endpointIdentity = await this.ExecuteInContextAsync(async dbContext =>
            {
                var endpointGuid = await dbContext.EndpointToCatchmentMappings.Where(x => x.CatchmentGUID == catchment.Value).Select(x => x.EndpointGUID).SingleAsync();

                var output = EndpointIdentity.From(endpointGuid);
                return output;
            });

            return endpointIdentity;
        }

        public async Task<EndpointTypeIdentity> GetEndpointType(EndpointIdentity endpoint)
        {
            var endpointTypeIdentity = await this.ExecuteInContextAsync(async dbContext =>
            {
                var endpointTypeGuid = await dbContext.GetEndpoint(endpoint).Select(x => x.EndpointType.GUID).SingleAsync();

                var output = EndpointTypeIdentity.From(endpointTypeGuid);
                return output;
            });

            return endpointTypeIdentity;
        }

        public async Task<EndpointIdentity> New()
        {
            var endpointIdentity = EndpointIdentity.New();

            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = new EndpointEntity()
                {
                    GUID = endpointIdentity.Value,
                };

                dbContext.Endpoints.Add(entity);

                await dbContext.SaveChangesAsync();
            });

            return endpointIdentity;
        }

        public async Task<EndpointIdentity> AddByName(EndpointInfo endpointInfo)
        {
            var endpointIdentity = EndpointIdentity.New();

            await this.ExecuteInContextAsync(async dbContext =>
            {
                var endpointTypeID = await dbContext.EndpointTypes.Where(x => x.Name == endpointInfo.TypeInfo.Name).Select(x => x.ID).SingleAsync();

                var endpointEntity = new Entities.Endpoint()
                {
                    GUID = endpointIdentity.Value,
                    Name = endpointInfo.Name,
                    EndpointTypeID = endpointTypeID,
                };

                dbContext.Endpoints.Add(endpointEntity);

                await dbContext.SaveChangesAsync();
            });

            return endpointIdentity;
        }

        public async Task SetEndpointForCatchment(CatchmentIdentity catchment, EndpointIdentity endpoint)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var mapping = await dbContext.EndpointToCatchmentMappings.Where(x => x.EndpointGUID == endpoint.Value).SingleOrDefaultAsync();

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

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task SetEndpointType(EndpointIdentity endpoint, EndpointTypeIdentity endpointType)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var getEndpointTypeID = dbContext.GetEndpointType(endpointType).AsNoTracking().Select(x => x.ID).SingleAsync();

                var getEndpointEntity = dbContext.Endpoints.Where(x => x.GUID == endpoint.Value).SingleAsync();

                var endpointTypeID = await getEndpointTypeID;
                var endpointEntity = await getEndpointEntity;

                endpointEntity.EndpointTypeID = endpointTypeID;

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task<EndpointInfo> GetInfo(EndpointIdentity identity)
        {
            var endpointInfo = await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.GetEndpoint(identity).Include(x => x.EndpointType).SingleAsync();

                var output = entity.ToAppType();
                return output;
            });

            return endpointInfo;
        }

        public async Task<IEnumerable<EndpointInfo>> GetAllInfos()
        {
            var endpointInfos = await this.ExecuteInContextAsync(async dbContext =>
            {
                var endpoints = await dbContext.Endpoints.Include(x => x.EndpointType).ToListAsync();

                var outputs = endpoints.Select(x => x.ToAppType());
                return outputs;
            });

            return endpointInfos;
        }

        public async Task<bool> CatchmentHasEndpoint(CatchmentIdentity catchmentIdentity)
        {
            var hasEndpoint = await this.ExecuteInContextAsync(async dbContext =>
            {
                var mappingEntityOrDefault = await dbContext.EndpointToCatchmentMappings.Where(x => x.CatchmentGUID == catchmentIdentity.Value).SingleOrDefaultAsync();

                var output = mappingEntityOrDefault is object;
                return output;
            });

            return hasEndpoint;
        }

        public async Task SetNameAsync(EndpointIdentity endpoint, string name)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var endpointEntity = await dbContext.GetEndpoint(endpoint).SingleAsync();

                endpointEntity.Name = name;

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task SetEndpointTypeAsync(EndpointIdentity endpoint, EndpointTypeIdentity endpointType)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var endpointTypeID = await dbContext.GetEndpointType(endpointType).AsNoTracking().Select(x => x.ID).SingleAsync();

                var endpointEntity = await dbContext.GetEndpoint(endpoint).SingleAsync();

                endpointEntity.EndpointTypeID = endpointTypeID;

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task<bool> EndpointHasCatchment(EndpointIdentity endpointIdentity)
        {
            var hasCatchment = await this.ExecuteInContextAsync(async dbContext =>
            {
                var mappingEntityOrDefault = await dbContext.EndpointToCatchmentMappings.Where(x => x.EndpointGUID == endpointIdentity.Value).SingleOrDefaultAsync();

                var output = mappingEntityOrDefault is object;
                return output;
            });

            return hasCatchment;
        }

        public async Task<CatchmentIdentity> GetCatchmentForEndpoint(EndpointIdentity endpointIdentity)
        {
            var catchmentIdentity = await this.ExecuteInContextAsync(async dbContext =>
            {
                var catchmentIdentityValue = await dbContext.EndpointToCatchmentMappings.Where(x => x.EndpointGUID == endpointIdentity.Value).Select(x => x.CatchmentGUID).SingleAsync();

                var output = CatchmentIdentity.From(catchmentIdentityValue);
                return output;
            });

            return catchmentIdentity;
        }
    }
}
