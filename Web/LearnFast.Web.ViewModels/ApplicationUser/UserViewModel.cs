namespace LearnFast.Web.ViewModels.ApplicationUser
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection.Metadata;
    using LearnFast.Common;
    using LearnFast.Web.ViewModels.Country;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Review;
    using Microsoft.AspNetCore.Mvc;

    public class UserViewModel : BaseUserViewModel
    {
        [Display(Name = GlobalConstants.WebsiteLabel)]
        [BindProperty(Name = "url")]
        public string WebsitePath { get; set; }

        public bool IsOwner { get; set; }

        public string Email { get; set; }

        public string Biography { get; set; }

        public CountryViewModel Country { get; set; }

        public int OwnCoursesCount => this.OwnCourses.Count();

        public int BuyedCoursesCount => this.BuyedCourses.Count();

        public int ReviewsCount => this.Reviews.Count();

        public IEnumerable<CourseProfileViewModel> OwnCourses { get; set; }

        public IEnumerable<EnrolledCourseViewModel> BuyedCourses { get; set; }

        public IEnumerable<ReviewProfileViewModel> Reviews { get; set; }
    }
}
