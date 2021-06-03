using CamundaInsurance.Data.Models;
using CamundaInsurance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Pages.Blazor.Card.Components
{
    [Authorize]
    public partial class UserInfoComponent
    {
        [Inject]
        private IdentityService IdentityService { get; set; }

        private User CurrentUser { get; set; }

        private List<string> Errors { get; set; } = new List<string>();

        protected async override Task OnParametersSetAsync()
        {
            var identityResponce = await IdentityService.GetCurrentUserAsync();
            if (identityResponce.Succeeded)
            {
                CurrentUser = identityResponce.Content;
            }
            else
            {
                Errors.AddRange(identityResponce.Messages);
            }                 
            await base.OnParametersSetAsync();
        }
    }
}
