using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Elders.Cronus.Serialization.NewtonsoftJson
{
    public sealed class ContractsRepository
    {
        private static readonly ILogger logger = CronusLogger.CreateLogger(typeof(ContractsRepository));

        readonly Dictionary<Type, string> typeToName = new Dictionary<Type, string>();
        readonly Dictionary<string, Type> nameToType = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public ContractsRepository(IEnumerable<Type> contracts)
        {
            if (contracts is not null)
            {
                StringBuilder contractErrors = null;
                foreach (var contract in contracts)
                {
                    if (contract.HasAttribute<DataContractAttribute>())
                    {
                        DataContractAttribute attribute = (DataContractAttribute)contract.GetCustomAttributes(typeof(DataContractAttribute), false).SingleOrDefault();
                        if (attribute is null || string.IsNullOrEmpty(attribute.Name))
                        {
                            if (contractErrors is null)
                            {
                                contractErrors = new StringBuilder();
                                contractErrors.AppendLine("The following types are missing `DataContractAttribute.Name` and they will NOT be added to the serializer's contracts repository. Usually you will here see types which are not part of your solution but dependencies. However it is worth checking the list bellow when something is not working properly.");
                            }
                            contractErrors.AppendLine(contract.FullName);
                            continue;
                        }

                        if (contract.IsGenericType)
                        {
                            var genericDefType = contract.GetGenericArguments().FirstOrDefault();
                            DataContractAttribute genattribute = (DataContractAttribute)genericDefType.GetCustomAttributes(typeof(DataContractAttribute), false).SingleOrDefault();

                            if (genattribute is not null && attribute.Name != genattribute.Name)
                            {

                                string contractString = $"{attribute.Name}_{genattribute.Name}";
                                Map(contract, contractString);
                            }
                        }
                        else
                        {
                            Map(contract, attribute.Name);
                        }
                    }
                }

                if (contractErrors is not null && contractErrors.Length > 1)
                    logger.Warn(() => contractErrors.ToString());
            }
        }

        internal string GetGenericTypeContract(Type type)
        {
            string contractId = string.Empty;

            DataContractAttribute attribute = (DataContractAttribute)type.GetCustomAttributes(typeof(DataContractAttribute), false).SingleOrDefault();
            if (attribute is not null && string.IsNullOrEmpty(attribute.Name) == false && type.IsGenericType)
            {
                var genericDefType = type.GetGenericArguments().FirstOrDefault();
                DataContractAttribute genattribute = (DataContractAttribute)genericDefType.GetCustomAttributes(typeof(DataContractAttribute), false).SingleOrDefault();

                if (genattribute is not null && attribute.Name != genattribute.Name)
                {
                    contractId = $"{attribute.Name}_{genattribute.Name}";
                }
            }

            return contractId;
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

        internal void Map(Type type, string name)
        {
            if (nameToType.ContainsKey(name) && nameToType[name] != type)
                throw new InvalidOperationException(string.Format("Duplicate contract registration {0}", name));

            typeToName.Add(type, name);
            nameToType.Add(name, type);
        }
    }
}
