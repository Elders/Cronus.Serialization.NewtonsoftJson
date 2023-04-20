using System;
using System.Collections.Generic;
using System.Linq;
using Elders.Cronus.Discoveries;
using Elders.Cronus.Serialization.NewtonsoftJson;

namespace Elders.Cronus.Pipeline.Config
{
    public class JsonSerializerDiscovery : DiscoveryBase<ISerializer>
    {
        protected override DiscoveryResult<ISerializer> DiscoverFromAssemblies(DiscoveryContext context)
        {
            return new DiscoveryResult<ISerializer>(GetModels(context));
        }

        IEnumerable<DiscoveredModel> GetModels(DiscoveryContext context)
        {
            yield return new DiscoveredModel(typeof(ISerializer), GetSerializer(context)); // Singleton
        }

        protected virtual ISerializer GetSerializer(DiscoveryContext context)
        {
            IEnumerable<Type> contracts = context.Assemblies
                .SelectMany(ass => ass.GetLoadableTypes());

            return new JsonSerializer(contracts);
        }
    }
}

