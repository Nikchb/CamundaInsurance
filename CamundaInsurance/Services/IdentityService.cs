using CamundaInsurance.Data;
using CamundaInsurance.Data.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Services
{
    public class IdentityService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ApplicationDbContext context;

        public IdentityService(AuthenticationStateProvider authenticationStateProvider, ApplicationDbContext context)
        {
            this.authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> IsUserAuthenticated()
        {
            return (await authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated == false)
            {
                throw new Exception("User is not authenticated");
            }
            var user = await context.Users.FirstOrDefaultAsync(v=>v.UserName == authState.User.Identity.Name);
            if(user == null)
            {
                throw new Exception("User is not found");
            }
            return user;
        }
    }
}
