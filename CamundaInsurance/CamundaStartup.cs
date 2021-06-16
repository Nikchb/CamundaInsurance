using Camunda.Api.Client;
using Camunda.Api.Client.Deployment;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CamundaInsurance
{
    public static class CamundaStartup
    {
        public async static Task WaitForCamundaAsync()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("CAMUNDA_URL") ?? "localhost:8080"}");

                while (true)
                {
                    try
                    {
                        var result = await httpClient.GetAsync("/engine-rest/user/count");
                        if (result.IsSuccessStatusCode)
                        {
                            return;
                        }                        
                    }
                    catch
                    {

                    }
                    await Task.Delay(1000);
                }
            }
        }

        public async static Task ConfigureCamundaAsync()
        {
            var camundaClient = CamundaClient.Create($"http://{Environment.GetEnvironmentVariable("CAMUNDA_URL") ?? "localhost:8080"}/engine-rest");
            await camundaClient.Deployments.Create(
                    deploymentName: "InsuranceRequestHandling",
                    duplicateFiltering: true,
                    changedOnly: true,
                    deploymentSource: "WebApp",
                    resources: new ResourceDataContent(File.OpenRead("BusinessProcesses/InsuranceRequestHandling.bpmn"), "InsuranceRequestHandling.bpmn"));
            await camundaClient.Deployments.Create(
                   deploymentName: "InsuranceRequestHandling",
                   duplicateFiltering: true,
                   changedOnly: true,
                   deploymentSource: "WebApp",
                   resources: new ResourceDataContent(File.OpenRead("BusinessProcesses/RiskCalculation.dmn"), "RiskCalculation.dmn"));

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("CAMUNDA_URL") ?? "localhost:8080"}");

                string status;
                var result = await httpClient.GetAsync("/engine-rest/user/count");
                if (result.IsSuccessStatusCode)
                {
                    var dic = JsonSerializer.Deserialize<Dictionary<string, int>>(await result.Content.ReadAsStringAsync());
                    status = dic["count"] > 1 ? "AlreadyConfugured" : "Confugure";
                }
                else
                {
                    throw new Exception("Service unavailable");
                }
                
                if (status == "AlreadyConfugured")
                {
                    return;
                }

                //creare permitions
                await httpClient.PostAsJsonAsync("/engine-rest/authorization/create",
                    new
                    {
                        type = 0,
                        resourceType = 0,
                        resourceId = "tasklist",
                        permissions = new string[] { "ALL" },
                        userId = "*"
                    });
                await httpClient.PostAsJsonAsync("/engine-rest/authorization/create",
                    new
                    {
                        type = 0,
                        resourceType = 5,
                        resourceId = "*",
                        permissions = new string[] { "ALL" },
                        userId = "*"
                    });

                //create groups
                await httpClient.PostAsJsonAsync("/engine-rest/group/create",
                    new
                    {
                        id = "underwritingClerks",
                        name = "Underwriting clerks"
                    });
                await httpClient.PostAsJsonAsync("/engine-rest/group/create",
                    new
                    {
                        id = "insuranceOfficers",
                        name = "Insurance officers"
                    });
                await httpClient.PostAsJsonAsync("/engine-rest/group/create",
                    new
                    {
                        id = "headOfTheUnderwritingDepartment",
                        name = "Head of the underwriting department"
                    });

                //create users
                await httpClient.PostAsJsonAsync("/engine-rest/user/create",
                    new
                    {
                        profile = new
                        {
                            id = "johnJohnson",
                            firstName = "John",
                            lastName = "Johnson"
                        },
                        credentials = new
                        {
                            password = "123456"
                        }
                    });
                await httpClient.PutAsJsonAsync("/engine-rest/group/underwritingClerks/members/johnJohnson", new { });

                await httpClient.PostAsJsonAsync("/engine-rest/user/create",
                    new
                    {
                        profile = new
                        {
                            id = "bobBrown",
                            firstName = "Bob",
                            lastName = "Brown"
                        },
                        credentials = new
                        {
                            password = "123456"
                        }
                    });
                await httpClient.PutAsJsonAsync("/engine-rest/group/insuranceOfficers/members/bobBrown", new { });

                await httpClient.PostAsJsonAsync("/engine-rest/user/create",
                    new
                    {
                        profile = new
                        {
                            id = "tomLee",
                            firstName = "Tom",
                            lastName = "Lee"
                        },
                        credentials = new
                        {
                            password = "123456"
                        }
                    });
                await httpClient.PutAsJsonAsync("/engine-rest/group/headOfTheUnderwritingDepartment/members/tomLee", new { });
            }
        }
    }
}
