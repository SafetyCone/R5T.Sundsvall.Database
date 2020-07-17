using System;

using Microsoft.EntityFrameworkCore;


namespace R5T.Sundsvall.Database
{
    public interface IEndpointDbContext
    {
        DbSet<Entities.Endpoint> Endpoints { get; }
        DbSet<Entities.EndpointType> EndpointTypes { get; }

        DbSet<Entities.EndpointToCatchmentMapping> EndpointToCatchmentMappings { get; }
    }
}
