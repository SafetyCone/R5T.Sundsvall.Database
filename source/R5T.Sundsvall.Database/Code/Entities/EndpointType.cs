using System;


namespace R5T.Sundsvall.Database.Entities
{
    public class EndpointType
    {
        public int ID { get; set; }

        public Guid GUID { get; set; }

        /// <summary>
        /// Not unique. Can be multiple endpoints with the same name.
        /// </summary>
        public string Name { get; set; }
    }
}
