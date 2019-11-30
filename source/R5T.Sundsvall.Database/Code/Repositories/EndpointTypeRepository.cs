using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using R5T.Endalia;
using R5T.Venetia;

using EndpointTypeEntity = R5T.Sundsvall.Database.Entities.EndpointType;


namespace R5T.Sundsvall.Database
{
    public class EndpointTypeRepository : DatabaseRepositoryBase<EndpointDbContext>, IEndpointTypeRepository
    {
        public EndpointTypeRepository(DbContextOptions<EndpointDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public override EndpointDbContext GetNewDbContext()
        {
            var dbContext = new EndpointDbContext(this.DbContextOptions);
            return dbContext;
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
            throw new NotImplementedException();
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
