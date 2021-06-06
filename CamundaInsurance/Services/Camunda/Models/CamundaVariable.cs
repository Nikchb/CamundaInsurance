using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CamundaInsurance.Services.Camunda.Models
{
    public class CamundaVariable
    {
        public CamundaVariable()
        {

        }

        public CamundaVariable(object value, string type)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public object Value { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public static class CamundaVariableTypes
    {
        public static string String => "string";
        public static string Long => "long";
        public static string Date => "date";
    }
}
