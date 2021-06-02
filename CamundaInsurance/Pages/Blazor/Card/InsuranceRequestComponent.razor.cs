using CamundaInsurance.Data;
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
    public partial class InsuranceRequestComponent
    {
        [Inject]
        private InsuranceManager InsuranceManager { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public INotifiableComponent ParentComponent { get; set; }

        private InsuranceRequestModel Model { get; set; } = new InsuranceRequestModel();

        private List<string> Errors { get; set; } = new List<string>();

        private async Task SendRequest()
        {
            Errors.Clear();
            var responce = await InsuranceManager.SendInsuranceRequest(Model);
            if(responce.Succeeded == false)
            {
                Errors.AddRange(responce.Messages);
            }
            ParentComponent.Notify();
        }
    }
}
