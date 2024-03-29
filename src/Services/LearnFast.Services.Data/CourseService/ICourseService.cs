﻿namespace LearnFast.Services.Data.CourseService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Filter;

    public interface ICourseService
    {
        IQueryable<T> GetAllAsQueryAble<T>();

        Task SearchCourses(SearchViewModel model);

        Task<ImportCourseModel> AddCourseAsync(ImportCourseModel course);

        Task DeleteCourseByIdAsync(int courseId, string userId);

        Task UpdateAsync(ImportCourseModel model, string userId);

        Task<string> GetOwnerIdByCourse(int courseId);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<int> GetCountAsync();

        Task EnrollCourse(int courseId, string userId);
    }
}
