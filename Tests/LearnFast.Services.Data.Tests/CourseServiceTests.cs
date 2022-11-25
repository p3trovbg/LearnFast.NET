namespace LearnFast.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using EllipticCurve;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class CourseServiceTests : BaseServiceTests
    {
        private Mock<IDeletableEntityRepository<Course>> repository;

        public CourseServiceTests()
        {
            this.repository = new Mock<IDeletableEntityRepository<Course>>();
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectNumber()
        {
            var courses = new List<Course>
                                          {
                                              new Course(),
                                              new Course(),
                                              new Course(),
                                          }.AsQueryable();

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null, null, null);

            Assert.Equal(courses.Count(), await service.GetCountAsync());

            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task AddCourseShouldReturnsValidIdOfAddedCourse()
        {
            var course = this.CreateModel();

            var list = new List<Course>();

            this.repository.Setup(m => m.AddAsync(It.IsAny<Course>()))
            .Callback(() => { return; });
            this.repository.Setup(m => m.SaveChangesAsync()).Callback(() => { return; });
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(this.Mapper, this.repository.Object, null, null, null, null, null, null);
            var result = await service.AddCourseAsync(course);
            var count = await service.GetCountAsync();

            this.repository.Verify(m => m.AddAsync(It.IsAny<Course>()), Times.Once());
            this.repository.Verify(m => m.SaveChangesAsync(), Times.Once());

            Assert.Equal(result, course.Id);
        }

        private ImportCourseModel CreateModel()
        {
            return new ImportCourseModel
            {
                Id = 1,
                Title = "Test",
                Price = 20,
                MainImageUrl = null,
                IsFree = false,
                CategoryId = 10,
                Difficulty = 1,
                LanguageId = 186,
                Requirments = "test",
                Description = "test",
            };
        }
    }
}
