using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Add(EndpointTypeInfo endpointType)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = new EndpointTypeEntity()
                {
                    Name = endpointType.Name,
                    GUID = endpointType.Identity,
                };

                dbContext.EndpointTypes.Add(entity);

                dbContext.SaveChanges();
            });
        }

        public void Delete(EndpointTypeIdentity identity)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.EndpointTypes.Where(x => x.GUID == identity.Value).Single();

                dbContext.Remove(entity);

                dbContext.SaveChanges();
            });
        }

        public bool Exists(EndpointTypeIdentity identity)
        {
            var exists = this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.EndpointTypes.Where(x => x.GUID == identity.Value).SingleOrDefault();

                var output = entity is object;
                return output;
            });

            return exists;
        }

        public IEnumerable<EndpointTypeInfo> GetAllInfos()
        {
            var endpointTypeInfos = this.ExecuteInContext(dbContext =>
            {
                var outputs = dbContext.EndpointTypes.ToList().Select(x =>
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

        public EndpointTypeInfo GetInfo(EndpointTypeIdentity identity)
        {
            var endpointTypeInfo = this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.EndpointTypes.Where(x => x.GUID == identity.Value).SingleOrDefault();

                var output = new EndpointTypeInfo()
                {
                    Identity = entity.GUID,
                    Name = entity.Name,
                };

                return output;
            });

            return endpointTypeInfo;
        }

        public string GetName(EndpointTypeIdentity identity)
        {
            var name = this.ExecuteInContext(dbContext =>
            {
                var output = dbContext.EndpointTypes.Where(x => x.GUID == identity.Value).Select(x => x.Name).Single();
                return output;
            });

            return name;
        }

        public void SetName(EndpointTypeIdentity identity, string name)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.EndpointTypes.Where(x => x.GUID == identity.Value).Single();

                entity.Name = name;

                dbContext.SaveChanges();
            });
        }

        public EndpointTypeIdentity New()
        {
            var endpointTypeIdentity = EndpointTypeIdentity.New();

            this.ExecuteInContext(dbContext =>
            {
                var entity = new EndpointTypeEntity()
                {
                    GUID = endpointTypeIdentity.Value,
                };

                dbContext.EndpointTypes.Add(entity);

                dbContext.SaveChanges();
            });

            return endpointTypeIdentity;
        }
    }
}
