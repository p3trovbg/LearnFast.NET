﻿namespace LearnFast.Services.Data.CourseService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Course;

    public interface IFilterCourse
    {
        Task<T> GetCourseByIdAsync<T>(int courseId);

        Task<IEnumerable<T>> GetTop12BestSellersCourses<T>();

        Task<IEnumerable<T>> GetOwnCoursesAsync<T>(string userId);

        bool IsUserEnrolledCourse(string userId, int courseId);
    }
}
