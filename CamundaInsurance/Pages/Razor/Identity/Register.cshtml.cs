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
        [Display(Name = "Gender")]
        [BindProperty]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [BindProperty]
        public DateTime BirthDay { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Post Index")]
        [MinLength(5)]
        [MaxLength(5)]
        [RegularExpression("^[0-9]*$")]
        [BindProperty]
        public string PostIndex { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        [BindProperty]
        public string City { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Street")]
        [BindProperty]
        public string Street { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "House Number")]
        [BindProperty]
        public string HouseNumber { get; set; }

        public void OnGet()
        {           
        }

        public async Task<IActionResult> OnPostAsync()
        {                   
            if (ModelState.IsValid)
            {
                var user = new User { 
                    UserName = Email,                    
                    BirthDay = BirthDay,                    
                    Name = Name,
                    SurName = Surname,
                    Gender = Gender,
                    PostIndex = PostIndex,
                    City = City,
                    Street = Street,
                    HouseNumber = HouseNumber
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
