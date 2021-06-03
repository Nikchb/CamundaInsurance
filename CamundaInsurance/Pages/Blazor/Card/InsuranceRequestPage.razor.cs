using CamundaInsurance.Services.Insurance.Models
using CamundaInsurance.Services.Insurance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CamundaInsurance.Pages.Blazor.Card
{
    [Authorize]
    public partial class InsuranceRequestPage
    {
        [Inject]
        private InsuranceManager InsuranceManager { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }       

        private InsuranceRequestModel Model { get; set; } = new InsuranceRequestModel();

        private List<string> Errors { get; set; } = new List<string>();

        private async Task SendRequest()
        {
            Errors.Clear();
            var responce = await InsuranceManager.SendInsuranceRequest(Model);
            if(responce.Succeeded)
            {
                NavigationManager.NavigateTo("/card");
                return;
            }
            Errors.AddRange(responce.Messages);
        }
    }
}
