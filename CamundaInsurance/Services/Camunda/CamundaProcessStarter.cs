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
        private readonly string returnUrl;

        public CamundaProcessStarter()
        {
            camundaUrl = Environment.GetEnvironmentVariable("CAMUNDA_URL") ?? "localhost:8080";
            returnUrl = "http://" + (Environment.GetEnvironmentVariable("CAMUNDA_RETURN_URL") ?? "webapp:80") + "/api/insurance";
        }

        public async Task<SResponce> StartProcess(string processKey, StartProcessModel model)
        {
            model.Add("returnUrl", returnUrl);
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
