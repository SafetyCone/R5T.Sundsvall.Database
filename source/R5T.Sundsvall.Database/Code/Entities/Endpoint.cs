using System;


namespace R5T.Sundsvall.Database.Entities
{
    public class Endpoint
    {
        public int ID { get; set; }

        public Guid GUID { get; set; }

        public string Name { get; set; }

        public int EndpointTypeID { get; set; }
        public EndpointType EndpointType { get; set; }
    }
}
