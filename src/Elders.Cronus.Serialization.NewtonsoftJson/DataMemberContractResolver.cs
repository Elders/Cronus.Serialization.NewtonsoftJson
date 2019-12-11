using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Elders.Cronus.Serialization.Newtonsofst.Jsson
{
    public class DataMemberContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty customMember = base.CreateProperty(member, memberSerialization);

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

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var allProperties = base.CreateProperties(type, memberSerialization);
            if (allProperties.Any() == false)
                return allProperties;

            for (int i = allProperties.Count - 1; i >= 0; i--)
            {
                JsonProperty currentProperty = allProperties[i];
                if (currentProperty.HasMemberAttribute)
                {
                    var memberInfo = type.GetAllMembers().Where(m => m.Name.Equals(currentProperty.PropertyName)).SingleOrDefault();
                    if (memberInfo is null == false)
                    {
                        JsonProperty newProperty = CreateProperty(memberInfo, memberSerialization);
                        newProperty.PropertyName = currentProperty.Order.ToString();

                        allProperties.Add(newProperty);
                    }
                }
            }

            return allProperties;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            if (objectType.GetInterfaces().Any(i => i == typeof(IDictionary) ||
                (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>))))
            {
                return base.CreateArrayContract(objectType);
            }

            return base.CreateContract(objectType);
        }
    }
}
