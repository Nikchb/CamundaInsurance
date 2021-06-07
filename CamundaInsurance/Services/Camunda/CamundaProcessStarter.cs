using CamundaInsurance.Services.Camunda.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CamundaInsurance.Services.Camunda
{
    public class CamundaProcessStarter : ServiceBase
    {
        private readonly string camundaUrl;

        public CamundaProcessStarter()
        {
            camundaUrl = Environment.GetEnvironmentVariable("CAMUNDA_URL") ?? "localhost:8080";            
        }

        public async Task<SResponce> StartProcess(string processKey, StartProcessModel model)
        {
            using (var httpClient = new HttpClient())
            {
                var responce = await httpClient.PostAsJsonAsync($"http://{camundaUrl}/engine-rest/process-definition/key/{processKey}/start", model);
                Console.WriteLine(JsonSerializer.Serialize(model));
                if(responce.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else
                {
                    return Error(responce.Content.ToString());
                }
                
            }
        }
    }


}
