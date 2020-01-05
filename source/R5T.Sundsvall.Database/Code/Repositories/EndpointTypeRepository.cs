using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using R5T.Endalia;
using R5T.Venetia;

using EndpointTypeEntity = R5T.Sundsvall.Database.Entities.EndpointType;


namespace R5T.Sundsvall.Database
{
    public class EndpointTypeRepository<TDbContext> : ProvidedDatabaseRepositoryBase<TDbContext>, IEndpointTypeRepository
        where TDbContext: DbContext, IEndpointDbContext
    {
        public EndpointTypeRepository(DbContextOptions<TDbContext> dbContextOptions, IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextOptions, dbContextProvider)
        {
        }

        public async Task Add(EndpointTypeInfo endpointType)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = new EndpointTypeEntity()
                {
                    Name = endpointType.Name,
                    GUID = endpointType.Identity,
                };

                dbContext.EndpointTypes.Add(entity);

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task Delete(EndpointTypeIdentity identity)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.GetEndpointType(identity).SingleAsync();

                dbContext.Remove(entity);

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task<bool> Exists(EndpointTypeIdentity identity)
        {
            var exists = await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.GetEndpointType(identity).SingleOrDefaultAsync();

                var output = entity is object;
                return output;
            });

            return exists;
        }

        public async Task<IEnumerable<EndpointTypeInfo>> GetAllInfos()
        {
            var endpointTypeInfos = await this.ExecuteInContextAsync(async dbContext =>
            {
                var endpointTypes = await dbContext.EndpointTypes.ToListAsync();

                var outputs = endpointTypes.Select(x =>
                {
                    var endpointTypeInfo = new EndpointTypeInfo()
                    {
                        Identity = x.GUID,
                        Name = x.Name,
                    };

                    return endpointTypeInfo;
                });

                return outputs;
            });

            return endpointTypeInfos;
        }

        public async Task<EndpointTypeInfo> GetInfo(EndpointTypeIdentity identity)
        {
            var endpointTypeInfo = await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.GetEndpointType(identity).SingleOrDefaultAsync();

                var output = new EndpointTypeInfo()
                {
                    Identity = entity.GUID,
                    Name = entity.Name,
                };

                return output;
            });

            return endpointTypeInfo;
        }

        public async Task<string> GetName(EndpointTypeIdentity identity)
        {
            var name = await this.ExecuteInContextAsync(async dbContext =>
            {
                var output = await dbContext.GetEndpointType(identity).Select(x => x.Name).SingleAsync();
                return output;
            });

            return name;
        }

        public async Task SetName(EndpointTypeIdentity identity, string name)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.GetEndpointType(identity).SingleAsync();

                entity.Name = name;

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task<EndpointTypeIdentity> New()
        {
            var endpointTypeIdentity = EndpointTypeIdentity.New();

            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = new EndpointTypeEntity()
                {
                    GUID = endpointTypeIdentity.Value,
                };

                dbContext.EndpointTypes.Add(entity);

                await dbContext.SaveChangesAsync();
            });

            return endpointTypeIdentity;
        }
    }
}
