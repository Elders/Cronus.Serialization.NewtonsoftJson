using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Elders.Cronus.Serialization.Newtonsofst.Jsson
{
    public class DataMemberContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty customMember = base.CreateProperty(member, memberSerialization);
            if (member.HasAttribute<DataMemberAttribute>())
                customMember.PropertyName = customMember.Order.ToString();

            if (!customMember.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    customMember.Writable = hasPrivateSetter;
                }
            }

            return customMember;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType.GetInterfaces().Any(i => i == typeof(IDictionary) ||
                (i.IsGenericType &&
                 i.GetGenericTypeDefinition() == typeof(IDictionary<,>))))
            {
                return base.CreateArrayContract(objectType);
            }

            return base.CreateContract(objectType);
        }
    }
}
