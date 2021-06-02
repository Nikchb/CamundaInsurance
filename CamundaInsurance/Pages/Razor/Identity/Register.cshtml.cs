using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CamundaInsurance.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace CamundaInsurance.Pages.Razor.Identity
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [BindProperty]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [BindProperty]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [BindProperty]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Name")]
        [BindProperty]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [BindProperty]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [BindProperty]
        public DateTime BirthDay { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Insutance Card Number")]
        [MinLength(8)]
        [MaxLength(8)]
        [RegularExpression("^[0-9]*$")]
        [BindProperty]
        public string InsuranceCardNumber { get; set; }

        public void OnGet()
        {           
        }

        public async Task<IActionResult> OnPostAsync()
        {                   
            if (ModelState.IsValid)
            {
                var user = new User { 
                    UserName = Email, 
                    Email = Email.Trim(), 
                    BirthDay = BirthDay,
                    InsuranceCardNumber = InsuranceCardNumber,
                    Name = Name,
                    SurName = Surname
                };
                var result = await _userManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("/");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }            
            return Page();
        }
    }
}
