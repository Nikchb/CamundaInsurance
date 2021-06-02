using CamundaInsurance.Data.Models;
using CamundaInsurance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Pages.Blazor.Card
{
    [Authorize]
    public partial class UserInfoComponent
    {
        [Inject]
        private IdentityService IdentityService { get; set; }

        private User CurrentUser { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            CurrentUser = await IdentityService.GetCurrentUserAsync();
            await base.OnParametersSetAsync();
        }
    }
}
