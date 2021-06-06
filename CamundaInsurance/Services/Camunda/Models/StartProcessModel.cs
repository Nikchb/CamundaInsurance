using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CamundaInsurance.Services.Camunda.Models
{
    public class StartProcessModel
    {
        [JsonProperty("businessKey")]
        [JsonPropertyName("businessKey")]
        public string BusinessKey { get; set; } = "Underfined";

        [JsonProperty("withVariablesInReturn")]
        [JsonPropertyName("withVariablesInReturn")]
        public bool WithVariablesInReturn = false;

        [JsonProperty("variables")]
        [JsonPropertyName("variables")]
        public Dictionary<string, CamundaVariable> Variables { get; set; } = new Dictionary<string, CamundaVariable>();

        public void Add(string key, CamundaVariable variable)
        {
            Variables.Add(key, variable);
        }

        public void Add(string key, object variable)
        {
            var type = CamundaVariableTypes.String;
            if(typeof(DateTime) == variable.GetType())
            {
                type = CamundaVariableTypes.Date;
                Add(key, new CamundaVariable(((DateTime)variable).ToString("yyyy-MM-dd'T'HH:mm:ss") + ".165+0100", type));
                return;
            }
            if (typeof(int) == variable.GetType() || typeof(long) == variable.GetType() || typeof(short) == variable.GetType())
            {
                type = CamundaVariableTypes.Long;
            }
            Add(key, new CamundaVariable(variable, type));
        }
    }
}
