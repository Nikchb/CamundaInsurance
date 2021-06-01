using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Pages
{
    [Authorize]
    public partial class AuthorizedComponentBase : ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationProvider { get; set; }       

        protected async Task<string> GetUserNameAsync()
        {
            var state = await AuthenticationProvider.GetAuthenticationStateAsync();
            return state.User.Identity.Name;
        }
    }
}
