namespace LearnFast.Web.ViewModels.Course
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Services.Mapping.PropertyCopier;
    using Microsoft.AspNetCore.Http;

    using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

    public class ImportCourseModel : IMapFrom<Course>
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course title")]
        public string Title { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [AllowHtml]
        [MaxLength(3000)]
        public string Description { get; set; }

        [Display(Name = "Upload image")]
        public IFormFile MainImage { get; set; }

        [Required]
        [AllowHtml]
        [MaxLength(3000)]
        public string Requirments { get; set; }

        [Display(Name = "Is free")]
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
