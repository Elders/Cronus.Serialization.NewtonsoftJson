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
            IEnumerable<Type> contracts = assemblies
                .Where(asm => ReferenceEquals(default(BoundedContextAttribute), asm.GetAssemblyAttribute<BoundedContextAttribute>()) == false)
                .SelectMany(ass => ass.GetExportedTypes());

            var serializer = new JsonSerializer(contracts);
            builder.Container.RegisterSingleton<ISerializer>(() => serializer);
        }
    }

    public static class PublisherSettingsExtensions
    {
        [Obsolete("Use T UseContractsFromAssemblies<T>(this T self, IEnumerable<Type> contracts). Will be removed in version 3.0.0")]
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
