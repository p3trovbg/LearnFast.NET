namespace LearnFast.Web.ViewModels.Course
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.ApplicationUser;
    using LearnFast.Web.ViewModels.Category;
    using LearnFast.Web.ViewModels.Language;
    using LearnFast.Web.ViewModels.Review;
    using Microsoft.AspNetCore.Http;

    public class BaseCourseViewModel : IMapFrom<Course>
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public IFormFile MainImage { get; set; }

        [Display(Name = "Free:")]
        public bool IsFree { get; set; }

        public int Difficulty { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Duration { get; set; }

        public LanguageViewModel Language { get; set; }

        public CategoryViewModel Category { get; set; }

        public BaseUserViewModel Owner { get; set; }
    }
}
