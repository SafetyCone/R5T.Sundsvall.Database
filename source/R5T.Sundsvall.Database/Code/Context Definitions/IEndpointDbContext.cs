using System;

using Microsoft.EntityFrameworkCore;


namespace R5T.Sundsvall.Database
{
    public interface IEndpointDbContext
    {
        DbSet<Entities.Endpoint> Endpoints { get; set; }
        DbSet<Entities.EndpointType> EndpointTypes { get; set; }

        DbSet<Entities.EndpointToCatchmentMapping> EndpointToCatchmentMappings { get; set; }
    }
}
