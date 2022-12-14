namespace LearnFast.Web.ViewModels
{
    using System;

    public class PagingViewModel
    {
        private const int Items = 9;

        public int? Page { get; set; }

        public bool HasPreviousPage => this.Page > 1;

        public bool HasNextPage => this.Page < this.PagesCount;

        public int? PreviousPage => this.Page - 1;

        public int? NextPage => this.Page + 1;

        public int TotalCount { get; set; }

        public int ItemsPerPage => Items;

        public int PagesCount => (int)Math.Ceiling((double)this.TotalCount / this.ItemsPerPage);
    }
}
