// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

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
using EducateMe.Web.Infrastructure.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EducateMe.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly IUserEmailStore<ApplicationUser> emailStore;
        private readonly ICitiesService citiesService;
        private readonly IInterestsService interestsService;
        private readonly ICategoriesService categoriesService;
        private readonly IStudentsService studentsService;
        private readonly IUsersService usersService;
        private readonly IAzureStorage azureStorage;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ICitiesService citiesService,
            IInterestsService interestsService,
            ICategoriesService categoriesService,
            IStudentsService studentsService,
            IUsersService usersService,
            IAzureStorage azureStorage)
        {
            this.userManager = userManager;
            this.userStore = userStore;
            this.emailStore = this.GetEmailStore();
            this.signInManager = signInManager;
            this.citiesService = citiesService;
            this.interestsService = interestsService;
            this.categoriesService = categoriesService;
            this.studentsService = studentsService;
            this.usersService = usersService;
            this.azureStorage = azureStorage;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Не сте въвели име")]
            [StringLength(25)]
            [MinLength(2)]
            [Display(Name = "Име")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Не сте въвели презиме")]
            [StringLength(25)]
            [MinLength(2)]
            [Display(Name = "Презиме")]
            public string MiddleName { get; set; }

            [Required(ErrorMessage = "Не сте въвели фамилия")]
            [StringLength(25)]
            [MinLength(2)]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Не сте въвели учебна институция")]
            [StringLength(75)]
            [MinLength(2)]
            [Display(Name = "Учебна институция")]
            public string School { get; set; }

            [Required(ErrorMessage = "Не сте въвели статус")]
            [StringLength(25)]
            [MinLength(2)]
            [Display(Name = "Статус")]
            public string Status { get; set; }

            [Required(ErrorMessage = "Не сте въвели години")]
            [Range(7, 100, ErrorMessage = "Допустимите стойности са между 7 и 100")]
            [Display(Name = "Години")]
            public int Age { get; set; }

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

            public List<SelectListItem> Interests { get; set; }

            [Display(Name = "Интереси")]
            [EnsureOneElement(ErrorMessage = "Не сте избрали интерес")]
            public List<int> InterestsIds { get; set; } = new();

            public List<SelectListItem> Categories { get; set; }

            [Display(Name = "Категории")]
            [EnsureOneElement(ErrorMessage = "Не сте избрали категория")]
            public List<int> CategoriesIds { get; set; } = new();

            [Required(ErrorMessage = "Не сте качили снимка")]
            [Display(Name = "Снимка")]
            public IFormFile Image { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.Input = new InputModel();

            // await this.PopulateInputModel(this.Input);
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
                    var student = new Student()
                    {
                        FirstName = this.Input.FirstName,
                        MiddleName = this.Input.MiddleName,
                        LastName = this.Input.LastName,
                        Age = this.Input.Age,
                        School = this.Input.School,
                        Status = this.Input.Status,
                        CityId = this.Input.CityId,
                    };

                    var imageResult = await this.azureStorage.UploadAsync(this.Input.Image);
                    if (!imageResult.Error)
                    {
                        student.ImageUrl = imageResult.Blob.Uri;
                    }
                    else
                    {
                        throw new ArgumentException("Error");
                    }

                    var addedStudent = await this.studentsService.CreateStudent(student, this.Input.InterestsIds, this.Input.CategoriesIds);

                    await this.usersService.SetUsersStudentId(user.Id, addedStudent.Id);
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.StudentRoleName);

                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    return this.LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            await this.PopulateInputModel(this.Input);

            // If we got this far, something failed, redisplay form
            return this.Page();
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

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!this.userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<ApplicationUser>)this.userStore;
        }

        private async Task PopulateInputModel(InputModel inputModel)
        {
            inputModel.Categories = (await this.categoriesService.GetCategories()).Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
            inputModel.Interests = (await this.interestsService.GetInterests()).Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            var provinces = await this.citiesService.GetProvinces();
            inputModel.Provinces = provinces.Select(x => new SelectListItem(x, x)).ToList();

            var cities = (await this.citiesService.GetCities(this.Input.Provinces[0].Value)).OrderBy(x => x.PostalCode);
            inputModel.Cities = cities.Select(x => new SelectListItem($"{x.Name}, {x.PostalCode}", x.Id.ToString())).ToList();
        }
    }
}
