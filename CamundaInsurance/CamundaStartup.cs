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
        public async static Task ConfigureCamunda()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("CAMUNDA_URL") ?? "localhost:8080"}");

                string status = null;
                while (status == null)
                {
                    var result = await httpClient.GetAsync("/engine-rest/user/count");
                    if(result.IsSuccessStatusCode)
                    {
                        var dic = JsonSerializer.Deserialize<Dictionary<string,int>>(await result.Content.ReadAsStringAsync());
                        status = dic["count"] > 1 ? "AlreadyConfugured" : "Confugure";
                    }
                    else
                    {
                        await Task.Delay(1000);
                    }

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
                        resourceType = 7,
                        resourceId = "*",
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

                //var data = await File.ReadAllBytesAsync("/app/BusinessProcesses/InsuranceRequestHandling.bpmn");

                //var requestContent = new MultipartFormDataContent();
                ////    here you can specify boundary if you need---^
                //var imageContent = new ByteArrayContent(ImageData);
                //imageContent.Headers.ContentType =
                //    MediaTypeHeaderValue.Parse("image/jpeg");

                //requestContent.Add(imageContent, "image", "image.jpg");

                //return await client.PostAsync(url, requestContent);
            }
        }
    }
}
