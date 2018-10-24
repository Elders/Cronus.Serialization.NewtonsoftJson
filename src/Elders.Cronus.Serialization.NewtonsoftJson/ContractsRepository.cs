using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Elders.Cronus.Serialization.NewtonsoftJson.Logging;

namespace Elders.Cronus.Serialization.NewtonsoftJson
{
    public class ContractsRepository
    {
        private static readonly ILog log = LogProvider.GetLogger(typeof(ContractsRepository));

        readonly Dictionary<Type, string> typeToName = new Dictionary<Type, string>();
        readonly Dictionary<string, Type> nameToType = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public ContractsRepository(IEnumerable<Type> contracts)
        {
            if (contracts != null)
            {
                foreach (var contract in contracts)
                {
                    if (contract.HasAttribute<DataContractAttribute>())
                    {
                        var contractName = contract.GetAttrubuteValue<DataContractAttribute, string>(x => x.Name);
                        if (string.IsNullOrEmpty(contractName))
                        {
                            log.Warn("Missing DataContractAttribute.Name for Type {0}", contract);
                            continue;
                        }
                        Map(contract, contractName);
                    }
                }
            }
        }

        public bool TryGet(Type type, out string name)
        {
            return typeToName.TryGetValue(type, out name);
        }

        public bool TryGet(string name, out Type type)
        {
            return nameToType.TryGetValue(name, out type);
        }

        public IEnumerable<Type> Contracts { get { return typeToName.Keys.ToList().AsReadOnly(); } }

        private void Map(Type type, string name)
        {
            if (nameToType.ContainsKey(name) && nameToType[name] != type)
                throw new InvalidOperationException(String.Format("Duplicate contract registration {0}", name));

            typeToName.Add(type, name);
            nameToType.Add(name, type);
        }
    }
}
