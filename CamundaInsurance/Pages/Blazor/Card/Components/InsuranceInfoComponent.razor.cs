using CamundaInsurance.Services.Insurance;
using CamundaInsurance.Services.Insurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Pages.Blazor.Card.Components
{
    [Authorize]
    public partial class InsuranceInfoComponent
    { 
        [Inject]
        private InsuranceManager InsuranceManager { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        private InsuranceInfoModel Info { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            var responce = await InsuranceManager.GetInsuranceInfoAsync();
            if(responce.Succeeded)
            {
                Info = responce.Content;
            }            
            await  base.OnParametersSetAsync();
        }

        private void RequestInsurance()
        {
            NavigationManager.NavigateTo("/card/new-request");
        }

        private void RevokeInsurance()
        {
            NavigationManager.NavigateTo("/card/revoke");
        }
    }
}
