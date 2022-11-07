// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace LearnFast.Web.Areas.Identity.Pages.Account.Manage
{
#nullable disable

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Threading.Tasks;

    using LearnFast.Common;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CountryService;
    using LearnFast.Services.Data.ImageService;
    using LearnFast.Web.ViewModels.Country;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ICountryService countryService;
        private readonly IImageService imageService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICountryService countryService,
            IImageService imageService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.countryService = countryService;
            this.imageService = imageService;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public IFormFile ImageFile { get; set; }

            [Display(Name = GlobalConstants.CountryLabel)]
            [Required]
            public int CountryId { get; set; }

            [StringLength(GlobalConstants.MaxWebsiteLength, MinimumLength = GlobalConstants.MinWebsiteLength)]
            [Display(Name = GlobalConstants.WebsiteLabel)]
            public string WebsiteUrl { get; set; }

            [Display(Name = GlobalConstants.BiographyLabel)]
            [StringLength(GlobalConstants.MaxBiographyLength)]
            public string Biography { get; set; }

            public IEnumerable<SelectListItem> Countries { get; set; }

            public string ImageUrl { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            if (this.Input.Biography != user.Biography)
            {
                user.Biography = this.Input.Biography;
            }

            if (this.Input.CountryId != user.CountryId)
            {
                user.CountryId = this.Input.CountryId;
            }

            if (this.Input.WebsiteUrl != user.WebsitePath)
            {
                user.WebsitePath = this.Input.WebsiteUrl;
            }

            if (this.Input.ImageFile != null)
            {
                var image = await this.imageService.UploadImage(this.Input.ImageFile, GlobalConstants.ImagesFolderName);
                user.MainImageUrl = image.UrlPath;
            }

            await this.userManager.UpdateAsync(user);

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var countries = this.countryService.GetAllOrderByAlphabetical<CountryViewModel>();

            this.Username = userName;

            this.Input = new InputModel
            {
                CountryId = user.CountryId,
                Biography = user.Biography,
                WebsiteUrl = user.WebsitePath,
                ImageUrl = user.MainImageUrl,
                Countries = countries.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };
        }
    }
}
