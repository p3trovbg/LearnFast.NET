namespace LearnFast.Web.ViewModels.Course
{
    using System.Collections.Generic;

    using AutoMapper;
    using LearnFast.Data.Models;
    using LearnFast.Services.Mapping;
    using LearnFast.Web.ViewModels.Review;

    public class CourseViewModel : BaseCourseViewModel
    {
        public string Description { get; set; }

        public string Requirments { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }


        // TODO: We should add course students list
    }
}
