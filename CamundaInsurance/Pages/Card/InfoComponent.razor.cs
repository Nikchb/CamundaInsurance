using CamundaInsurance.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Pages.Card
{
    [Authorize]
    public partial class InfoComponent
    {
        [Inject]
        private UserManager<User> UserManager { get; set; }

        private User CurrentUser { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            CurrentUser = await UserManager.FindByNameAsync(await GetUserNameAsync());
            await base.OnParametersSetAsync();
        }
    }
}
