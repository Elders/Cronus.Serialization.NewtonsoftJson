using System;
using System.Collections.Generic;
using System.Linq;
using Elders.Cronus.IocContainer;
using Elders.Cronus.Serialization.NewtonsoftJson;
using Elders.Cronus.Serializer;

namespace Elders.Cronus.Pipeline.Config
{
    public static class PublisherSettingsExtensions
    {
        public static T UseContractsFromAssemblies<T>(this T self, IEnumerable<Type> contracts)
            where T : ICanConfigureSerializer
        {
            var builder = self as ISettingsBuilder;
            var fullContracts = new List<Type>(contracts);
            fullContracts.AddRange(typeof(CronusAssembly).Assembly.GetExportedTypes());
            fullContracts.AddRange(typeof(IMessage).Assembly.GetExportedTypes());
            var serializer = new JsonSerializer(contracts.ToArray());
            builder.Container.RegisterSingleton<ISerializer>(() => serializer);
            return self;
        }
    }
}

