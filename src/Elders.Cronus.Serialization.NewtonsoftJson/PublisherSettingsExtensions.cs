using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Elders.Cronus.Discoveries;
using Elders.Cronus.IocContainer;
using Elders.Cronus.Serialization.NewtonsoftJson;
using Elders.Cronus.Serializer;

namespace Elders.Cronus.Pipeline.Config
{
    public class DefaultContractsAssembliesDiscovery : DiscoveryBasedOnExecutingDirAssemblies
    {
        protected override void DiscoverFromAssemblies(ISettingsBuilder builder, IEnumerable<Assembly> assemblies)
        {
            Assembly[] assembliesWithContracts = assemblies
                .Where(ass => ass.GetExportedTypes().Any(type => type.HasAttribute<DataContractAttribute>()))
                .ToArray();

            var serializer = new JsonSerializer(assembliesWithContracts);
            builder.Container.RegisterSingleton<ISerializer>(() => serializer);
        }
    }

    public static class PublisherSettingsExtensions
    {
        public static T UseContractsFromAssemblies<T>(this T self, Assembly[] assembliesContainingContracts = null)
            where T : ICanConfigureSerializer
        {
            var builder = self as ISettingsBuilder;
            var contracts = new List<Assembly>(assembliesContainingContracts);
            contracts.AddRange(new Assembly[] { typeof(CronusAssembly).Assembly, typeof(IMessage).Assembly });
            var serializer = new JsonSerializer(contracts.ToArray());
            builder.Container.RegisterSingleton<ISerializer>(() => serializer);
            return self;
        }
    }
}
