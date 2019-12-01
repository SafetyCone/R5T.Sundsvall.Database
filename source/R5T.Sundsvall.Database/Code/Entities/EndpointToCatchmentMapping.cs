using System;


namespace R5T.Sundsvall.Database.Entities
{
    public class EndpointToCatchmentMapping
    {
        public int ID { get; set; }

        public Guid EndpointGUID { get; set; }

        public Guid CatchmentGUID { get; set; }
    }
}
