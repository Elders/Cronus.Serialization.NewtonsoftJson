using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Elders.Cronus.Serialization.NewtonsoftJson
{
    public class ContractsRepository
    {
        private static readonly ILogger logger = CronusLogger.CreateLogger(typeof(ContractsRepository));

        readonly Dictionary<Type, string> typeToName = new Dictionary<Type, string>();
        readonly Dictionary<string, Type> nameToType = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public ContractsRepository(IEnumerable<Type> contracts)
        {
            if (contracts is not null)
            {
                StringBuilder contractErrors = new StringBuilder();
                contractErrors.AppendLine("The following types are missing `DataContractAttribute.Name` and they will NOT be added to the serializer's contracts repository. Usually you will here see types which are not part of your solution but dependencies. However it is worth checking the list bellow when something is not working properly.");
                foreach (var contract in contracts)
                {
                    if (contract.HasAttribute<DataContractAttribute>())
                    {
                        var contractName = contract.GetAttrubuteValue<DataContractAttribute, string>(x => x.Name);
                        if (string.IsNullOrEmpty(contractName))
                        {
                            contractErrors.AppendLine(contract.FullName);
                            continue;
                        }
                        Map(contract, contractName);
                    }
                }

                if (contractErrors.Length > 1)
                    logger.Warn(() => contractErrors.ToString());
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

        public IEnumerable<Type> Contracts { get { return typeToName.Keys; } }

        private void Map(Type type, string name)
        {
            if (nameToType.ContainsKey(name) && nameToType[name] != type)
                throw new InvalidOperationException(string.Format("Duplicate contract registration {0}", name));

            typeToName.Add(type, name);
            nameToType.Add(name, type);
        }
    }
}
