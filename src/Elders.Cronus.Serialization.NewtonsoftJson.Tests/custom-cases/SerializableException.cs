﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Machine.Specifications;

namespace Elders.Cronus.Serialization.NewtonsoftJson.Tests.custom_cases
{
    [DataContract(Name = "c3ca519f-5ee8-460e-8f7b-c8a84d2fd191")]
    public class SerializableException : Exception
    {
        SerializableException() { }

        //FIGURE THIS OUT
        protected SerializableException(SerializationInfo info, StreamingContext ctx)
        {
        }

        public SerializableException(Exception ex)
        {
            ExType = ex.GetType();
            ExMessage = ex.Message;
            ExStackTrace = ex.StackTrace;
            //ExTargetSite = ex.TargetSite.ToString();
            ExSource = ex.Source;
            ExHelpLink = ex.HelpLink;

            if (ex.InnerException != null)
                ExInnerException = new SerializableException(ex.InnerException);
        }

        [DataMember(Order = 1)]
        public Type ExType { get; private set; }

        [DataMember(Order = 2)]
        public string ExMessage { get; private set; }

        [DataMember(Order = 3)]
        public string ExStackTrace { get; private set; }
        //public string ExTargetSite { get; private set; }

        [DataMember(Order = 4)]
        public string ExSource { get; private set; }

        [DataMember(Order = 5)]
        public string ExHelpLink { get; private set; }

        [DataMember(Order = 100)]
        public SerializableException ExInnerException { get; set; }
    }
    [Subject(typeof(JsonSerializer))]
    public class Whem_SerializableException
    {

        Establish context = () =>
        {
            try
            {
                try
                {
                    throw new InvalidOperationException("inner");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("outer", ex);
                }
            }
            catch (Exception ex)
            {
                ser = new SerializableException(ex);
            }
            var contracts = new List<Type>();
            contracts.AddRange(typeof(NestedType).Assembly.GetExportedTypes());
            contracts.AddRange(typeof(Whem_SerializableException).Assembly.GetExportedTypes());
            serializer = new JsonSerializer(contracts);
            serializer2 = new JsonSerializer(contracts);
            serStream = new MemoryStream();
            serializer.Serialize(serStream, ser);
            serStream.Position = 0;
        };

        Because of_deserialization = () => { deser = (SerializableException)serializer.Deserialize(serStream); };

        It should_not_be_null = () => deser.ShouldNotBeNull();

        static SerializableException ser;
        static SerializableException deser;
        static Stream serStream;
        static JsonSerializer serializer;
        static JsonSerializer serializer2;
    }

}
