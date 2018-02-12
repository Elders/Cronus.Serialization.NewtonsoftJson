using System.Reflection;
using Elders.Cronus.Serializer;
using Elders.Cronus.IocContainer;
using Elders.Cronus.Serialization.NewtonsoftJson;
using System.Collections.Generic;

namespace Elders.Cronus.Pipeline.Config
{
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
