using FluentValidation;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly IValidator<InputModel> _validator;
        private readonly UserManager<ApplicationUser> _userManager;

        public Index(IValidator<InputModel> validator, UserManager<ApplicationUser> userManager)
        {
            _validator = validator;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public bool RegisterSuccess { get; set; }

        public async Task<IActionResult> OnGet(string returnUrl)
        {
            Input = new InputModel
            {
                ReturnUrl = returnUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (Input.Button != "register") return Redirect("~/");

            var modelValid = await this._validator.ValidateAsync(Input);
            if (!modelValid.IsValid)
            {
                modelValid.Errors.ForEach(error =>
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                });
                return Page();
            }

        }
    }
}
