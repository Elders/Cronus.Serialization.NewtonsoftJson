using Elders.Cronus.DomainModeling;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Models
{
    [DataContract(Name = "635c3731-2e25-4b47-a1ef-80a0f248092b")]
    public class EventWithSpecificAggregateRootId : IEvent
    {
        [DataMember(Order = 1)]
        public EventStoreIndexManagerId Id { get; set; }
    }

    [DataContract(Name = "8aa438ee-fbc8-4425-aa65-b71c7a3f09d8")]
    public class EventWithIAggregateRootId : IEvent
    {
        [DataMember(Order = 1)]
        public IAggregateRootId Id { get; set; }
    }

    [DataContract(Name = "b30ed6a4-beee-4561-b69b-d780dbbf5557")]
    public class EventStoreIndexManagerId : StringTenantId
    {
        EventStoreIndexManagerId() : base() { }

        public EventStoreIndexManagerId(string indexName, string tenant) : base(indexName, "eventstoreindexmanager", tenant) { }
        public EventStoreIndexManagerId(StringTenantUrn urn) : base(urn, "eventstoreindexmanager") { }
    }
}
