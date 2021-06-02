using CamundaInsurance.Models;
using CamundaInsurance.Pages.Blazor.Interfaces;
using CamundaInsurance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Pages.Blazor.Card
{
    [Authorize]
    public partial class InsuranceInfoComponent : INotifiableComponent
    {
        [Inject]
        private InsuranceManager InsuranceManager { get; set; }

        private InsuranceInfoModel Info { get; set; }

        public void Notify()
        {
            StateHasChanged();            
        }

        protected async override Task OnParametersSetAsync()
        {
            var responce = await InsuranceManager.GetInsuranceInfoAsync();
            if(responce.Succeeded)
            {
                Info = responce.Content;
            }            
            await  base.OnParametersSetAsync();
        }
    }
}
