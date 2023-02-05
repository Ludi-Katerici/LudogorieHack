// <copyright file="RegisterOrganization.cshtml.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EducateMe.Common;
using EducateMe.Data.Models;
using EducateMe.Services.Data.Interfaces;
using EducateMe.Web.AzureServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EducateMe.Web.Areas.Identity.Pages.Account;

public class RegisterOrganization : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IUserStore<ApplicationUser> userStore;
    private readonly IUserEmailStore<ApplicationUser> emailStore;
    private readonly IAzureStorage azureStorage;
    private readonly IOrganizationsService organizationsService;
    private readonly ICitiesService citiesService;
    private readonly IUsersService usersService;
    private readonly SignInManager<ApplicationUser> signInManager;

    public RegisterOrganization(
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        IAzureStorage azureStorage,
        IOrganizationsService organizationsService,
        ICitiesService citiesService,
        IUsersService usersService,
        SignInManager<ApplicationUser> signInManager)
    {
        this.userManager = userManager;
        this.userStore = userStore;
        this.emailStore = this.GetEmailStore();
        this.azureStorage = azureStorage;
        this.organizationsService = organizationsService;
        this.citiesService = citiesService;
        this.usersService = usersService;
        this.signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Не сте въвели име")]
        [StringLength(25)]
        [MinLength(3)]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не сте въвели име")]
        [StringLength(5000, ErrorMessage = "Максимум 5000 знака")]
        [MinLength(50, ErrorMessage = "Минимум 50 знака")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Не сте качили снимка")]
        [Display(Name = "Снимка")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Не сте въвел email")]
        [EmailAddress(ErrorMessage = "Не валиден email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не сте въвели парола")]
        [StringLength(100, ErrorMessage = "{0} трябва да бъде поне {2} и най-много {1} знаци дълга.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърдете паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; }

        public List<SelectListItem> Provinces { get; set; } = new();

        public List<SelectListItem> Cities { get; set; } = new();

        [Required(ErrorMessage = "Не сте избрали град")]
        [Display(Name = "Град")]
        public int CityId { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        this.Input = new InputModel();

        await this.PopulateInputModel(this.Input);

        this.ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= this.Url.Content("~/");

        if (this.ModelState.IsValid)
        {
            var user = this.CreateUser();

            await this.userStore.SetUserNameAsync(user, this.Input.Email, CancellationToken.None);
            await this.emailStore.SetEmailAsync(user, this.Input.Email, CancellationToken.None);
            var result = await this.userManager.CreateAsync(user, this.Input.Password);

            if (result.Succeeded)
            {
                var organization = new Organization()
                {
                    Name = this.Input.Name,
                    Description = this.Input.Description,
                    CityId = this.Input.CityId,
                };

                var imageResult = await this.azureStorage.UploadAsync(this.Input.Image);
                if (!imageResult.Error)
                {
                    organization.ImageUrl = imageResult.Blob.Uri;
                }
                else
                {
                    throw new ArgumentException("Error");
                }

                organization = await this.organizationsService.CreateOrganization(organization);

                await this.usersService.SetUsersOrganizationId(user.Id, organization.Id);
                await this.userManager.AddToRoleAsync(user, GlobalConstants.OrganizationRoleName);

                await this.signInManager.SignInAsync(user, isPersistent: false);
                return this.LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        await this.PopulateInputModel(this.Input);

        return this.Page();
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!this.userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<ApplicationUser>)this.userStore;
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException(
                $"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/RegisterUser.cshtml");
        }
    }

    private async Task PopulateInputModel(InputModel inputModel)
    {
        var provinces = await this.citiesService.GetProvinces();
        inputModel.Provinces = provinces.Select(x => new SelectListItem(x, x)).ToList();

        var cities = (await this.citiesService.GetCities(this.Input.Provinces[0].Value)).OrderBy(x => x.PostalCode);
        inputModel.Cities = cities.Select(x => new SelectListItem($"{x.Name}, {x.PostalCode}", x.Id.ToString())).ToList();
    }
}
