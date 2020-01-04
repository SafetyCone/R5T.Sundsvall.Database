using System;

using R5T.Endalia;

using EntityType = R5T.Sundsvall.Database.Entities.Endpoint;


namespace R5T.Sundsvall.Database
{
    public static class EndpointExtensions
    {
        public static EndpointInfo ToAppType(this EntityType entity)
        {
            var endpointTypeInfo = entity.EndpointType is object
                ? new EndpointTypeInfo()
                {
                    Identity = entity.EndpointType.GUID,
                    Name = entity.EndpointType.Name,
                }
                : null;

            var endpointInfo = new EndpointInfo()
            {
                Identity = entity.GUID,
                Name = entity.Name,
                TypeInfo = endpointTypeInfo
            };

            return endpointInfo;
        }
    }
}
