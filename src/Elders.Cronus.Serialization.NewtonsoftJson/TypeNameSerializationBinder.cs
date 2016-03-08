using System.Reflection;
using Elders.Cronus.Serializer;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NewtonsoftJson = Newtonsoft.Json.Serialization;
using System;

namespace Elders.Cronus.Serialization.NewtonsoftJson
{
    public class TypeNameSerializationBinder : SerializationBinder
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TypeNameSerializationBinder));

        static Assembly NetAssembly = typeof(object).Assembly;

        private readonly ContractsRepository contractRepository;

        public TypeNameSerializationBinder(Assembly[] contractAssemblyes)
        {
            this.contractRepository = new ContractsRepository(contractAssemblyes);
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
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

        public override Type BindToType(string assemblyName, string typeName)
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
            catch (TypeLoadException ex)
            {
                string error = String.Format("Cannot resolve type {0}:'{1}'. Probably the type was renamed or an object was serialized without DataContractAttribute on first place. In order to not break the rest of the results this record will not be deserialized and default value will be returned.", assemblyName, typeName);
                throw new InvalidOperationException(error);
            }
        }
    }
}