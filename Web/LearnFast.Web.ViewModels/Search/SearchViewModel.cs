namespace LearnFast.Web.ViewModels.Filter
{
    using System.Collections.Generic;

    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SearchViewModel : PagingViewModel
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

        public Dictionary<string, string> Params => new Dictionary<string, string>()
        {
            { nameof(this.SearchString), this.SearchString },
            { nameof(this.LanguageId), this.LanguageId.ToString() },
            { nameof(this.CategoryId), this.CategoryId.ToString() },
            { nameof(this.Difficulty), this.Difficulty.ToString() },
            { nameof(this.InitialPrice), this.InitialPrice.ToString() },
            { nameof(this.FinalPrice), this.FinalPrice.ToString() },
            { nameof(this.SorterArgument), this.SorterArgument },
            { nameof(this.IsFree), this.IsFree.ToString() },
        };
    }
}
