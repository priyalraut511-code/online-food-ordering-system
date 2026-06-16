#nullable disable

using FoodOrderSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync()
        {
            ExternalLogins =
                (await _signInManager
                .GetExternalAuthenticationSchemesAsync())
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(
                        Input.Email,
                        Input.Password,
                        Input.RememberMe,
                        lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user =
                        await _userManager.FindByEmailAsync(
                            Input.Email);

                    if (user.Role == "Admin")
                    {
                        return RedirectToAction(
                            "Dashboard",
                            "Admin");
                    }

                    return RedirectToAction(
                        "Index",
                        "Home");
                }

                ModelState.AddModelError(
                    string.Empty,
                    "Invalid Login Attempt");
            }

            return Page();
        }
    }
}