using CamundaInsurance.Services.Insurance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Pages.Blazor.Card
{
    [Authorize]
    public partial class InsuranceRevokePage
    {
        [Inject]
        private InsuranceManager InsuranceManager { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }      

        private List<string> Errors { get; set; } = new List<string>();

        private async Task Revoke()
        {
            Errors.Clear();
            var responce = await InsuranceManager.RevokeInsurance();
            if(responce.Succeeded)
            {
                NavigationManager.NavigateTo("/card");
                return;
            }
            Errors.AddRange(responce.Messages);
        }

        private void No()
        {
            NavigationManager.NavigateTo("/card");
        }
    }
}
