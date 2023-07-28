using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests.custom_cases
{
    [DataContract(Name = "b1ff3d28-1744-41ef-8f63-a1b4394bcc2b")]
    public class UserConversations
    {
        public UserConversations()
        {
            Holder = new UserConversationsHolder();

        }
        public UserConversations(UserId id)
        {
            Id = id;
            Holder = new UserConversationsHolder();
        }

        [DataMember(Order = 1)]
        public virtual UserId Id { get; set; }
        [DataMember(Order = 2)]
        public virtual UserConversationsHolder Holder { get; set; }
        [DataMember(Order = 3)]
        public virtual DateTime UpdatedAt { get; set; }
    }

    [DataContract(Name = "bc0b0ded-1042-46db-a8d3-9fe85fba2d06")]
    public class UserId : AggregateRootId
    {
        protected UserId() { }
        public UserId(Guid id) : base(id.ToString(), "User", "elders") { }
    }

    [DataContract(Name = "20d462ee-3340-4a93-9e9f-b74b2a373881")]
    public class ConversationId : AggregateRootId
    {
        protected ConversationId() { }
        public ConversationId(Guid id) : base(id.ToString(), "Conversation", "elders") { }
    }

    [DataContract(Name = "4b1ab5ff-e7b5-48f4-bceb-a85671fd281f")]
    public class UserConversationsHolder
    {
        public UserConversationsHolder()
        {
            Conversations = "test";
        }
        [DataMember(Order = 1)]
        public virtual string Conversations { get; set; }
    }

    [DataContract(Name = "4591e41a-e8be-46d4-906f-3f23bfb8fe36")]
    public class ConversationParticipantItem
    {
        protected ConversationParticipantItem() { }

        public ConversationParticipantItem(ConversationId id, UserId participant, bool isOwner, DateTime timestamp)
        {
            ConversationId = id;
            ParticipantId = participant;
            IsOwner = isOwner;
            Timestamp = timestamp;
        }

        [DataMember(Order = 1)]
        public virtual ConversationId ConversationId { get; set; }

        [DataMember(Order = 2)]
        public virtual UserId ParticipantId { get; set; }

        [DataMember(Order = 3)]
        public virtual bool IsOwner { get; set; }

        public virtual DateTime Timestamp { get; set; }
    }

    [Subject(typeof(JsonSerializer))]
    public class Whem_UserConversations
    {
        Establish context = () =>
        {
            var ser = new UserConversations(new UserId(Guid.NewGuid())) { UpdatedAt = DateTime.UtcNow };
            ser.Holder.Conversations = "asd";
            var contracts = new List<Type>();
            contracts.AddRange(typeof(NestedType).Assembly.GetExportedTypes());
            contracts.AddRange(typeof(Whem_UserConversations).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);

            data = serializer.SerializeToBytes(ser);
        };
        Because of_deserialization = () => { deser = serializer.DeserializeFromBytes<UserConversations>(data); };

        It should_not_be_null = () => deser.ShouldNotBeNull();

        static UserConversations deser;
        static JsonSerializer serializer;
        static byte[] data;
    }
}
