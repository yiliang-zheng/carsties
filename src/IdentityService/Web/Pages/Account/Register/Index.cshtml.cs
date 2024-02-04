using Application.RegisterUser;
using Domain.ApplicationUser;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly IValidator<InputModel> _validator;
        private readonly ISender _sender;

        public Index(IValidator<InputModel> validator, ISender sender)
        {
            _validator = validator;
            _sender = sender;
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

            var registerUserCommand = new RegisterUserCommand
            {
                Email = Input.Email,
                FullName = Input.FullName,
                Password = Input.Password,
                UserName = Input.Username
            };
            var result = await this._sender.Send(registerUserCommand);
            if (result.IsFailed)
            {
                result.Errors.ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                });
                return Page();
            }

            this.RegisterSuccess = true;
            return Page();
        }
    }
}
