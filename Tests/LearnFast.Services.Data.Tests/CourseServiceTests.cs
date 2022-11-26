namespace LearnFast.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using AngleSharp.Text;
    using EllipticCurve;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Web.ViewModels.ApplicationUser;
    using LearnFast.Web.ViewModels.Course;
    using Microsoft.CodeAnalysis.VisualBasic.Syntax;
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
            .Callback(() => { list.Add(this.Mapper.Map<Course>(course)); });
            this.repository.Setup(m => m.SaveChangesAsync()).Callback(() => { return; });
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(this.Mapper, this.repository.Object, null, null, null, null, null, null);
            var result = await service.AddCourseAsync(course);

            this.repository.Verify(m => m.AddAsync(It.IsAny<Course>()), Times.Once());
            this.repository.Verify(m => m.SaveChangesAsync(), Times.Once());

            Assert.Equal(result, course.Id);
        }

        [Fact]
        public async Task AddCourseShouldIncreasesCountWithOne()
        {
            var course = this.CreateModel();

            var list = new List<Course>();

            this.repository.Setup(m => m.AddAsync(It.IsAny<Course>()))
            .Callback(() => { list.Add(this.Mapper.Map<Course>(course)); });
            this.repository.Setup(m => m.SaveChangesAsync()).Callback(() => { return; });
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(this.Mapper, this.repository.Object, null, null, null, null, null, null);
            var result = await service.GetCountAsync();
            Assert.Equal(list.Count(), result);

            await service.AddCourseAsync(course);
            var secondResult = await service.GetCountAsync();

            Assert.Equal(list.Count(), secondResult);
        }

        [Fact]
        public async Task DeleteCourseById()
        {
            var course = new Course { Id = 5, Owner = new ApplicationUser() { Id = "22" } };

            var list = new List<Course>();
            list.Add(course);

            var count = list.Count();

            this.repository.Setup(m => m.Delete(It.IsAny<Course>()))
            .Callback(() => { list.Remove(course); });
            this.repository.Setup(m => m.SaveChangesAsync()).Callback(() => { return; });
            this.repository.Setup(r => r.All()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null, null, null);

            await service.DeleteCourseByIdAsync(course.Id, course.Owner.Id);

            this.repository.Verify(m => m.Delete(It.IsAny<Course>()), Times.Once());
            this.repository.Verify(m => m.SaveChangesAsync(), Times.Once());

            Assert.NotEqual(count, list.Count());
        }

        [Fact]
        public async Task DeleteCourseByInvalidIdShouldThrowsException()
        {
            var course = new Course { Id = 5, Owner = new ApplicationUser() { Id = "22" } };

            var list = new List<Course>();
            list.Add(course);

            this.repository.Setup(r => r.All()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null, null, null);

            var ex = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeleteCourseByIdAsync(3, course.Owner.Id));
            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeleteCourseByIdAsync(4, course.Owner.Id));

            Assert.Equal(GlobalExceptions.CourseIsNotExistExceptionMessage, ex.Message);
        }

        [Fact]
        public async Task DeleteCourseByIdShouldThrowsExceptionIfUserIsNotOwner()
        {
            var course = new Course { Id = 5, Owner = new ApplicationUser() { Id = "22" } };

            var list = new List<Course>();
            list.Add(course);
            this.repository.Setup(r => r.All()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null, null, null);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteCourseByIdAsync(course.Id, "100"));
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteCourseByIdAsync(course.Id, "30"));

            Assert.Equal(GlobalExceptions.DoesNotOwnThisCourseExceptionMessage, ex.Message);
        }

        [Fact]
        public async Task UpdateCourseByInvalidIdShouldThrowsException()
        {
            var updatedCourse = this.CreateModel();
            var course = new Course { Id = 51, Owner = new ApplicationUser() { Id = "22" } };

            var list = new List<Course>();
            list.Add(course);

            this.repository.Setup(r => r.All()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null, null, null);

            var ex = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.UpdateAsync(updatedCourse, course.Owner.Id));
            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.UpdateAsync(updatedCourse, course.Owner.Id));

            Assert.Equal(GlobalExceptions.CourseIsNotExistExceptionMessage, ex.Message);
        }

        [Fact]
        public async Task UpdateCourseByIdShouldThrowsExceptionIfUserIsNotOwner()
        {
            var updatedCourse = this.CreateModel();
            var course = new Course { Id = 5, Owner = new ApplicationUser() { Id = "22" } };

            var list = new List<Course>();
            list.Add(course);

            this.repository.Setup(r => r.All()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null, null, null);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.UpdateAsync(updatedCourse, "3"));
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.UpdateAsync(updatedCourse, "6"));

            Assert.Equal(GlobalExceptions.DoesNotOwnThisCourseExceptionMessage, ex.Message);
        }

        [Fact]
        public async Task UpdateCourseByIdShouldUpdateTheEntity()
        {
            var updatedCourse = this.CreateModel();

            var course = new Course { Id = 5, Owner = new ApplicationUser() { Id = "22" } };

            var list = new List<Course>();
            list.Add(course);

            this.repository.Setup(r => r.All()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null, null, null);

            await service.UpdateAsync(updatedCourse, course.Owner.Id);

            var model = this.Mapper.Map<Course>(updatedCourse);

            // I set null of owners because they can't update and don't need to compare themselves.
            course.Owner = null;
            model.Owner = null;

            Assert.Equivalent(model, course, strict: true);
        }

        [Fact]
        public async Task UpdateCourseByIdShouldSetIsFreeTrueIfThePriceIsZero()
        {
            var updatedCourse = this.CreateModel();

            updatedCourse.Price = 0;

            var course = new Course { Id = 5, Owner = new ApplicationUser() { Id = "22" } };

            var list = new List<Course>();
            list.Add(course);

            this.repository.Setup(r => r.All()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null, null, null);

            await service.UpdateAsync(updatedCourse, course.Owner.Id);

            Assert.True(course.IsFree);
            Assert.True(course.Price == 0);
        }

        [Fact]
        public async Task UpdateCourseByIdShouldSetIsFreeFalseIfThePriceIsBiggerMoreZero()
        {
            var updatedCourse = this.CreateModel();

            updatedCourse.IsFree = true;

            var course = new Course { Id = 5, Owner = new ApplicationUser() { Id = "22" } };

            var list = new List<Course>();
            list.Add(course);

            this.repository.Setup(r => r.All()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null, null, null);

            await service.UpdateAsync(updatedCourse, course.Owner.Id);

            Assert.False(course.IsFree);
            Assert.True(course.Price > 0);
        }

        private ImportCourseModel CreateModel()
        {
            var owner = new BaseUserViewModel { Id = "22" };
            return new ImportCourseModel
            {
                Owner = owner,
                Id = 5,
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
