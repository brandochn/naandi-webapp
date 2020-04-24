using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;
using System;

namespace WebApp.Data
{
    public class JsonNetSerializer : IRestSerializer
    {
        public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj);

        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value);

        public T Deserialize<T>(IRestResponse response)
        {
            try
            {
                T result = JsonConvert.DeserializeObject<T>(response.Content);
                return result;
            }
            catch(Exception)
            {
                return default;
            }
        }

        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;
    }
}