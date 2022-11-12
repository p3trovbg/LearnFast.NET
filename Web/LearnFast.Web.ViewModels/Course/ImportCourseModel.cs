namespace LearnFast.Web.ViewModels.Course
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    using LearnFast.Common;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Services.Mapping.PropertyCopier;
    using Microsoft.AspNetCore.Http;

    using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

    public class ImportCourseModel : IMapFrom<Course>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxCourseTitleLength, MinimumLength = GlobalConstants.MinCourseTitleLength)]
        [Display(Name = GlobalConstants.CourseTitleLabel)]
        public string Title { get; set; }

        [Required]
        [Range(typeof(decimal), GlobalConstants.MinCoursePrice, GlobalConstants.MaxCoursePrice)]
        public decimal Price { get; set; }

        [Required]
        [AllowHtml]
        [StringLength(GlobalConstants.MaxCourseDescriptionLength, MinimumLength = GlobalConstants.MinCourseDescriptionLength)]
        public string Description { get; set; }

        [Display(Name = GlobalConstants.UploadImageLabel)]
        public IFormFile MainImage { get; set; }

        [Required]
        [AllowHtml]
        [StringLength(GlobalConstants.MaxCourseRequirmentsLength, MinimumLength = GlobalConstants.MinCourseRequirmentsLength)]
        public string Requirments { get; set; }

        [Display(Name = GlobalConstants.IsFreeLabel)]
        public bool IsFree { get; set; }

        [Required]
        public int Difficulty { get; set; }

        [Required]
        public int LanguageId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [NotCopy]
        public ApplicationUser Owner { get; set; }

        public string MainImageUrl { get; set; }

        [NotCopy]
        [NotMapped]
        public IEnumerable<SelectListItem> Languages { get; set; }

        [NotMapped]
        [NotCopy]
        public IEnumerable<SelectListItem> Categories { get; set; }

        [NotMapped]
        [NotCopy]
        public IEnumerable<SelectListItem> Difficulties { get; set; }
    }
}
