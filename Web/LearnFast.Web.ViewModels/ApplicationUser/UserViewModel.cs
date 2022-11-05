namespace LearnFast.Web.ViewModels.ApplicationUser
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LearnFast.Data.Models;
    using LearnFast.Web.ViewModels.Country;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Mvc;

    public class UserViewModel : BaseUserViewModel
    {
        [Display(Name = "Website")]
        [BindProperty(Name = "url")]
        public string WebsitePath { get; set; }

        public string UserName { get; set; }

        public bool IsOwner { get; set; }

        public string Email { get; set; }

        public string Biography { get; set; }

        public CountryViewModel Country { get; set; }

        public IEnumerable<CourseProfileViewModel> OwnCourses { get; set; }

        public IEnumerable<EnrolledCourseViewModel> BuyedCourses { get; set; }
    }
}
