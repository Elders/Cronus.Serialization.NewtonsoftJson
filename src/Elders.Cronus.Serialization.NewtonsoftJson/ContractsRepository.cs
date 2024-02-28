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
        readonly Dictionary<string, Type> genericTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

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
                            genericTypes.Add(attribute.Name, contract);

                            string contractString = GetGenericTypeContract(contract);
                            if (string.IsNullOrEmpty(contractString) == false)
                            {
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
                Type[] genericArgumentTypes = type.GetGenericArguments();

                StringBuilder contractConstruction = new StringBuilder();
                contractConstruction.Append($"{attribute.Name}");

                foreach (Type genericArgumentType in genericArgumentTypes)
                {
                    DataContractAttribute genattribute = (DataContractAttribute)genericArgumentType.GetCustomAttributes(typeof(DataContractAttribute), false).SingleOrDefault();
                    if (genattribute is not null && attribute.Name != genattribute.Name)
                    {
                        contractConstruction.Append($"`{genattribute.Name}");
                    }
                    else
                    {
                        contractConstruction = contractConstruction.Clear();
                        break;
                    }
                }
                contractId = contractConstruction.ToString();
            }

            return contractId;
        }

        internal Type GetGenericType(string contract)
        {
            var types = contract.Split('`');
            if (types.Length < 2)
                return null;

            if (genericTypes.TryGetValue(types[0], out Type genericBase))
            {
                List<Type> genericArguments = new List<Type>(types.Length - 1);
                for (int i = 1; i < types.Length; i++)
                {
                    if (TryGet(types[i], out Type genericArg))
                    {
                        genericArguments.Add(genericArg);
                    }
                    else
                    {
                        return null;
                    }
                }

                return genericBase.MakeGenericType(genericArguments.ToArray());
            }

            return null;
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
