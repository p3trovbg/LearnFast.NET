namespace LearnFast.Web.ViewModels.Filter
{
    using System.Collections.Generic;

    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class FilterViewModel
    {
        public string? SearchString { get; set; }

        public int? LanguageId { get; set; }

        public int? CategoryId { get; set; }

        public int? Difficulty { get; set; }

        public decimal? InitialPrice { get; set; }

        public decimal? FinalPrice { get; set; }

        public bool IsFree { get; set; }

        public string? SorterArgument { get; set; }

        public string? CategoryName { get; set; }

        public IEnumerable<BaseCourseViewModel>? Courses { get; set; }

        public IEnumerable<SelectListItem>? Categories { get; set; }

        public IEnumerable<SelectListItem>? Difficulties { get; set; }

        public IEnumerable<SelectListItem>? Languages { get; set; }

        public IEnumerable<SelectListItem>? Sorter { get; set; }
    }
}
