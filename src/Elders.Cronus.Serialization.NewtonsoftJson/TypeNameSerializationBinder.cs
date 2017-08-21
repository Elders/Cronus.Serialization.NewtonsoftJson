using System.Reflection;
using System.Runtime.Serialization;
using System;
using Elders.Cronus.Serialization.NewtonsoftJson.Logging;
using Newtonsoft.Json.Serialization;

namespace Elders.Cronus.Serialization.NewtonsoftJson
{
    public class TypeNameSerializationBinder : ISerializationBinder
    {
        static ILog log = LogProvider.GetLogger(typeof(TypeNameSerializationBinder));

        static Assembly NetAssembly = typeof(object).Assembly;

        private readonly ContractsRepository contractRepository;

        public TypeNameSerializationBinder(Assembly[] contractAssemblyes)
        {
            this.contractRepository = new ContractsRepository(contractAssemblyes);
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
