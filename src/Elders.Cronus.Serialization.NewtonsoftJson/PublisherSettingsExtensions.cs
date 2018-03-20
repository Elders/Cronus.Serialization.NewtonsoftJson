using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Elders.Cronus.Discoveries;
using Elders.Cronus.IocContainer;
using Elders.Cronus.Serialization.NewtonsoftJson;
using Elders.Cronus.Serializer;
using Mono.Cecil;

namespace Elders.Cronus.Pipeline.Config
{
    public class DefaultContractsAssembliesDiscovery : DiscoveryBasedOnExecutingDirAssemblies
    {
        protected override void DiscoverFromAssemblies(ISettingsBuilder builder, IEnumerable<AssemblyDefinition> assemblies)
        {
            IEnumerable<Type> assembliesWithContracts = assemblies
                .SelectMany(asm => asm.Modules)
                .SelectMany(mod =>
                {
                    var td = mod.ImportReference(typeof(DataContractAttribute)).Resolve();
                    return mod.GetTypes();//.Where(type => type.CustomAttributes.Any(attr => attr.Constructor.DeclaringType == td));
                })
                .Select(dt => dt.ToType());

            var serializer = new JsonSerializer(assembliesWithContracts);
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

    public static class TypeDefinitionExtensions
    {
        /// <summary>
        /// Is childTypeDef a subclass of parentTypeDef. Does not test interface inheritance
        /// </summary>
        /// <param name="childTypeDef"></param>
        /// <param name="parentTypeDef"></param>
        /// <returns></returns>
        public static bool IsSubclassOf(this TypeDefinition childTypeDef, TypeDefinition parentTypeDef)
        {
            return
                childTypeDef.MetadataToken != parentTypeDef.MetadataToken && childTypeDef.EnumerateBaseClasses()
                .Any(b => b.MetadataToken == parentTypeDef.MetadataToken);
        }

        /// <summary>
        /// Does childType inherit from parentInterface
        /// </summary>
        /// <param name="childType"></param>
        /// <param name="parentInterfaceDef"></param>
        /// <returns></returns>
        public static bool DoesAnySubTypeImplementInterface(this TypeDefinition childType, TypeDefinition parentInterfaceDef)
        {
            return childType
               .EnumerateBaseClasses()
               .Any(typeDefinition => typeDefinition.DoesSpecificTypeImplementInterface(parentInterfaceDef));
        }

        /// <summary>
        /// Does the childType directly inherit from parentInterface. Base
        /// classes of childType are not tested
        /// </summary>
        /// <param name="childTypeDef"></param>
        /// <param name="parentInterfaceDef"></param>
        /// <returns></returns>
        public static bool DoesSpecificTypeImplementInterface(this TypeDefinition childTypeDef, TypeDefinition parentInterfaceDef)
        {
            return childTypeDef
               .Interfaces
               .Any(ifaceDef => DoesSpecificInterfaceImplementInterface(ifaceDef.InterfaceType.Resolve(), parentInterfaceDef));
        }

        /// <summary>
        /// Does interface iface0 equal or implement interface iface1
        /// </summary>
        /// <param name="iface0"></param>
        /// <param name="iface1"></param>
        /// <returns></returns>
        public static bool DoesSpecificInterfaceImplementInterface(TypeDefinition iface0, TypeDefinition iface1)
        {
            return iface0.MetadataToken == iface1.MetadataToken || iface0.DoesAnySubTypeImplementInterface(iface1);
        }

        /// <summary>
        /// Is source type assignable to target type
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsAssignableFrom(this TypeDefinition target, TypeDefinition source)
        {
            return
                target == source ||
                target.MetadataToken == source.MetadataToken ||
                source.IsSubclassOf(target) ||
                target.IsInterface && source.DoesAnySubTypeImplementInterface(target);
        }

        /// <summary>
        /// Enumerate the current type, it's parent and all the way to the top type
        /// </summary>
        /// <param name="klassType"></param>
        /// <returns></returns>
        public static IEnumerable<TypeDefinition> EnumerateBaseClasses(this TypeDefinition klassType)
        {
            for (var typeDefinition = klassType; typeDefinition != null; typeDefinition = typeDefinition.BaseType?.Resolve())
            {
                yield return typeDefinition;
            }
        }

        public static Type ToType(this TypeDefinition typeDef)
        {
            return Type.GetType(typeDef.FullName + ", " + typeDef.Module.Assembly.FullName);
        }
    }
}
