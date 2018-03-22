using System;
using System.Collections.Generic;
using System.Reflection;
using Elders.Cronus.Serialization.NewtonsoftJson.Logging;
using Newtonsoft.Json.Serialization;

namespace Elders.Cronus.Serialization.NewtonsoftJson
{
    public class TypeNameSerializationBinder : ISerializationBinder
    {
        static ILog log = LogProvider.GetLogger(typeof(TypeNameSerializationBinder));

        static Assembly NetAssembly = typeof(object).Assembly;

        private readonly ContractsRepository contractRepository;

        [Obsolete("Use TypeNameSerializationBinder(IEnumerable<Type> contracts). Will be removed in version 3.0.0")]
        public TypeNameSerializationBinder(Assembly[] contractAssemblies)
        {
            this.contractRepository = new ContractsRepository(contractAssemblies);
        }

        public TypeNameSerializationBinder(IEnumerable<Type> contracts)
        {
            this.contractRepository = new ContractsRepository(contracts);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            string name;
            if (contractRepository.TryGet(serializedType, out name))
            {
                assemblyName = null;
                typeName = name;
            }
            else
            {
                if (serializedType.Assembly == NetAssembly)
                {
                    assemblyName = serializedType.Assembly.FullName;
                    typeName = serializedType.FullName;
                }
                else
                    throw new InvalidOperationException(String.Format("Unkown, unregistered type {0}", serializedType));
            }
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            try
            {
                if (assemblyName == null)
                {
                    Type type;
                    if (contractRepository.TryGet(typeName, out type))
                        return type;
                }
                return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName), true);
            }
            catch (TypeLoadException)
            {
                throw new InvalidOperationException(String.Format("Unkown, unregistered type {0}:'{1}'", assemblyName, typeName));
            }
        }
    }
}
