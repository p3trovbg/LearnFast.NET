namespace LearnFast.Web.ViewModels.ApplicationUser
{
    using LearnFast.Web.ViewModels.Country;
    using LearnFast.Web.ViewModels.Review;

    public class UserViewModel : BaseUserViewModel
    {
        public string Biography { get; set; }

        public string PhoneNumber { get; set; }

        public CountryViewModel Country { get; set; }

        public CountryListViewModel OwnCourses { get; set; }

        public CountryListViewModel EnrolledCourses { get; set; }

        public ReviewListViewModel Reviews { get; set; }
    }
}
