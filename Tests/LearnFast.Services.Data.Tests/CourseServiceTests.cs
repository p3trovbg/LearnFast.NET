namespace LearnFast.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using AngleSharp.Text;
    using CloudinaryDotNet;
    using EllipticCurve;
    using LearnFast.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Migrations;
    using LearnFast.Data.Models;
    using LearnFast.Data.Models.Enums;
    using LearnFast.Data.Seeding.DTOs;
    using LearnFast.Services.Data.CategoryService;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Web.ViewModels.ApplicationUser;
    using LearnFast.Web.ViewModels.Course;
    using LearnFast.Web.ViewModels.Filter;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.CodeAnalysis.CSharp;
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

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

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

            var service = new CourseService(this.Mapper, this.repository.Object, null, null, null, null);
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

            var service = new CourseService(this.Mapper, this.repository.Object, null, null, null, null);
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

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

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

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var ex = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeleteCourseByIdAsync(3, course.Owner.Id));
            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeleteCourseByIdAsync(4, course.Owner.Id));

            Assert.Equal(GlobalExceptions.CourseDoesNotExistExceptionMessage, ex.Message);
        }

        [Fact]
        public async Task DeleteCourseByIdShouldThrowsExceptionIfUserIsNotOwner()
        {
            var course = new Course { Id = 5, Owner = new ApplicationUser() { Id = "22" } };

            var list = new List<Course>();
            list.Add(course);
            this.repository.Setup(r => r.All()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

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

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var ex = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.UpdateAsync(updatedCourse, course.Owner.Id));
            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.UpdateAsync(updatedCourse, course.Owner.Id));

            Assert.Equal(GlobalExceptions.CourseDoesNotExistExceptionMessage, ex.Message);
        }

        [Fact]
        public async Task UpdateCourseByIdShouldThrowsExceptionIfUserIsNotOwner()
        {
            var updatedCourse = this.CreateModel();
            var course = new Course { Id = 5, Owner = new ApplicationUser() { Id = "22" } };

            var list = new List<Course>();
            list.Add(course);

            this.repository.Setup(r => r.All()).Returns(list.AsQueryable().BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

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

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

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

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

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

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            await service.UpdateAsync(updatedCourse, course.Owner.Id);

            Assert.False(course.IsFree);
            Assert.True(course.Price > 0);
        }

        [Fact]
        public async Task GetAllShouldReturnsAllCourses()
        {
            var courses = GetCoursesCollection();
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var result = await service.GetAllAsync<CourseViewModel>();
            Assert.Equal(courses.Count(), result.Count());
            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            Assert.Equal(courses[0].Title, result.Where(x => x.Id == courses[0].Id).Select(x => x.Title).FirstOrDefault());
            Assert.Equal(courses[1].Requirments, result.Where(x => x.Id == courses[1].Id).Select(x => x.Requirments).FirstOrDefault());
        }

        [Fact]
        public async Task GetOwnedCoursesShouldReturnsOnlyCoursesOwnedByTheirOwner()
        {
            var courses = GetCoursesCollection();

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());
            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var owner = courses.Select(x => x.Owner).FirstOrDefault(x => x.Nickname == "peter");

            var result = await service.GetOwnCoursesAsync<BaseCourseViewModel>(owner.Id);

            var ownerCourses = courses.Where(x => x.Owner.Id == owner.Id).ToList();

            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            Assert.Equal(ownerCourses[0].Title, result.Where(x => x.Id == ownerCourses[0].Id).Select(x => x.Title).FirstOrDefault());
            Assert.Equal(ownerCourses[0].IsFree, result.Where(x => x.Id == ownerCourses[0].Id).Select(x => x.IsFree).FirstOrDefault());
        }

        [Fact]
        public void IsEnrolledCourseShouldReturnsTrueIfUserIsEnrolled()
        {
            var courses = GetCoursesCollection();

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());
            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var user = courses.Select(x => x.Owner).FirstOrDefault(x => x.Nickname == "peter");
            var courseId = 4;
            var result = service.IsUserEnrolledCourse(user.Id, courseId);

            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public void IsEnrolledCourseShouldReturnsFalseIfUserIsNotEnrolled()
        {
            var courses = GetCoursesCollection();

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());
            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var user = courses.Select(x => x.Owner).FirstOrDefault(x => x.Nickname == "peter");

            var courseId = 10;
            var result = service.IsUserEnrolledCourse(user.Id, courseId);

            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            Assert.False(result);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(6)]
        public async Task GetCourseByIdShouldReturnCurrentCourse(int courseId)
        {
            var courses = GetCoursesCollection();
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());
            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var result = await service.GetCourseByIdAsync<BaseCourseViewModel>(courseId);

            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            var selectCourse = courses.FirstOrDefault(x => x.Id == courseId);

            Assert.Equal(selectCourse.Title, result.Title);
            Assert.Equal(selectCourse.Id, result.Id);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(20)]
        [InlineData(10000000)]
        public async Task GetCourseByInvalidIdShouldThrowsException(int courseId)
        {
            var courses = GetCoursesCollection();
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());
            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var ex = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.GetCourseByIdAsync<BaseCourseViewModel>(courseId));
            Assert.Equal(GlobalExceptions.CourseDoesNotExistExceptionMessage, ex.Message);

            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllAsQueryableShouldReturnQueryableCollection()
        {
            var courses = GetCoursesCollection();
            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());
            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var result = service.GetAllAsQueryAble<BaseCourseViewModel>();

            Assert.IsAssignableFrom<IQueryable>(result);
            Assert.Equal(courses[0].Title, courses.Where(x => x.Id == courses[0].Id).Select(x => x.Title).FirstOrDefault());
            Assert.Equal(courses.Count(), result.Count());

            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task GetOwnerIdOfCourseShouldReturnsOwnerId()
        {
            var courses = GetCoursesCollection();
            var course = courses.FirstOrDefault();

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var result = await service.GetOwnerIdByCourse(course.Id);

            Assert.Equal(course.Owner.Id, result);
            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task GetOwnerIdOfCourseByInvalidCourseIdShouldThrowsException()
        {
            var courses = GetCoursesCollection();
            var course = courses.FirstOrDefault();

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetOwnerIdByCourse(-1));
            Assert.Equal(GlobalExceptions.CourseDoesNotExistExceptionMessage, ex.Message);

            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task EnrollingInCourseByUser()
        {
            var courses = GetCoursesCollection();
            var course = courses.FirstOrDefault(x => x.Id == 4);
            var candidate = new ApplicationUser() { FirstName = "Dragan", Nickname = "dragan" };

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());
            this.repository.Setup(r => r.All()).Returns(courses.BuildMock());
            this.repository.Setup(m => m.SaveChangesAsync()).Callback(()
                =>
                {
                    course.CourseStudents.Add(new StudentCourse { CourseId = course.Id, UserId = candidate.Id });
                });

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            await service.EnrollCourse(course.Id, candidate.Id);

            Assert.True(service.IsUserEnrolledCourse(candidate.Id, course.Id));
            this.repository.Verify(x => x.AllAsNoTracking(), Times.Exactly(2));
            this.repository.Verify(x => x.All(), Times.Once);
            this.repository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EnrollingInNotExistCourseShouldThrowException()
        {
            var courses = GetCoursesCollection();
            var course = courses.FirstOrDefault(x => x.Id == 4);
            var candidate = new ApplicationUser() { FirstName = "Dragan", Nickname = "dragan" };

            this.repository.Setup(r => r.All()).Returns(courses.BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var ex = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.EnrollCourse(-1, candidate.Id));

            Assert.Equal(GlobalExceptions.CourseDoesNotExistExceptionMessage, ex.Message);
            this.repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task EnrollingInCourseByUserThatAlreadyIsEnrolledShouldThrowException()
        {
            var courses = GetCoursesCollection();
            var course = courses.FirstOrDefault(x => x.Id == 4);
            var candidate = course.CourseStudents.Select(x => x.User).FirstOrDefault(x => x.Nickname == "gosho");

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());
            this.repository.Setup(r => r.All()).Returns(courses.BuildMock());

            var service = new CourseService(null, this.repository.Object, null, null, null, null);
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.EnrollCourse(course.Id, candidate.Id));

            Assert.Equal(GlobalExceptions.UserAlreadyHasEnrolledInCourse, ex.Message);
            this.repository.Verify(x => x.All(), Times.Once);
            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public async Task SearchCoursesByCategoryId(int categoryId)
        {
            var mockCategoryRepository = new Mock<IDeletableEntityRepository<Category>>();
            mockCategoryRepository.Setup(r => r.AllAsNoTracking())
                .Returns(CategoryServiceTests.GetCategoryList().BuildMock());

            var courses = GetCoursesCollection();
            var coursesByCategory = courses.Where(x => x.CategoryId == categoryId);

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.AsQueryable().BuildMock());

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var searchModel = new SearchViewModel()
            {
                CategoryId = categoryId,
                Courses = mappedCourses,
            };

            var categoryService = new CategoryService(mockCategoryRepository.Object);
            var service = new CourseService(null, this.repository.Object, null, categoryService, null, null);

            await service.SearchCourses(searchModel);

            Assert.Equal(coursesByCategory.Count(), searchModel.Courses.Count());
            Assert.All(searchModel.Courses, x => Assert.Equal(categoryId, x.Category.Id));
            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3000)]
        public async Task SearchCoursesByInvalidCategoryIdShouldThrowsException(int categoryId)
        {
            var courses = GetCoursesCollection();
            var coursesByCategory = courses.Where(x => x.CategoryId == categoryId);

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.AsQueryable().BuildMock());

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var searchModel = new SearchViewModel()
            {
                CategoryId = categoryId,
                Courses = mappedCourses,
            };

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.SearchCourses(searchModel));
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("Test1")]
        [InlineData("Test5")]
        public async Task FilterCoursesBySearchString(string searchString)
        {
            var courses = GetCoursesCollection();
            var coursesBySearchString = courses.Where(x => x.Title.Contains(searchString));

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var searchModel = new SearchViewModel()
            {
                SearchString = searchString,
                Courses = mappedCourses,
            };

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var result = service.Filter(searchModel, mappedCourses.AsQueryable());

            Assert.Equal(coursesBySearchString.Count(), result.Count());
            Assert.All(result, x => Assert.Contains(searchString, x.Title));
        }

        [Theory]
        [InlineData(50, 100)]
        [InlineData(100, 300)]
        public void FilterCoursesByPriceWithUpAndDownLimit(int minPrice, int maxPrice)
        {
            var courses = GetCoursesCollection();
            var coursesBySearchString = courses.Where(x => x.Price >= minPrice && x.Price <= maxPrice);

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var searchModel = new SearchViewModel()
            {
                InitialPrice = minPrice,
                FinalPrice = maxPrice,
                Courses = mappedCourses,
            };

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var result = service.Filter(searchModel, mappedCourses.AsQueryable());

            Assert.Equal(coursesBySearchString.Count(), result.Count());
            Assert.All(result, x => Assert.True(x.Price >= minPrice && x.Price <= maxPrice));
        }

        [Theory]
        [InlineData(50, 100)]
        public void FilterCoursesByPriceWithUpAndDownLimitShouldReturnsOnlyFreeCoursesIfIsFreeIsTrue(int minPrice, int maxPrice)
        {
            var courses = GetCoursesCollection();
            var coursesBySearchString = courses.Where(x => x.Price >= minPrice && x.Price <= maxPrice);

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var searchModel = new SearchViewModel()
            {
                InitialPrice = minPrice,
                FinalPrice = maxPrice,
                Courses = mappedCourses,
                IsFree = true,
            };

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var result = service.Filter(searchModel, mappedCourses.AsQueryable());

            Assert.All(result, x => Assert.True(x.IsFree));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void FilterCoursesByLanguage(int languageId)
        {
            var courses = GetCoursesCollection();
            var coursesByLanguage = courses.Where(x => x.LanguageId == languageId);

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var searchModel = new SearchViewModel()
            {
                LanguageId = languageId,
                Courses = mappedCourses,
            };

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            var result = service.Filter(searchModel, mappedCourses.AsQueryable());

            Assert.All(result, x => Assert.Equal(languageId, x.Language.Id));
        }

        [Fact]
        public void SorterCoursesOrderByTitle()
        {
            string sorter = GlobalConstants.OrderByTitle;
            var courses = GetCoursesCollection();

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var sortedCourses = mappedCourses.OrderBy(x => x.Title);

            var service = new CourseService(null, this.repository.Object, null, null, null, null);
            var result = service.Sorter(sorter, mappedCourses.AsQueryable());

            Assert.Equal(sortedCourses, result);
        }

        [Fact]
        public void SorterCoursesOrderByPrice()
        {
            string sorter = GlobalConstants.OrderByPrice;
            var courses = GetCoursesCollection();

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var sortedCourses = mappedCourses.OrderBy(x => x.Price);

            var service = new CourseService(null, this.repository.Object, null, null, null, null);
            var result = service.Sorter(sorter, mappedCourses.AsQueryable());

            Assert.Equal(sortedCourses, result);
        }

        [Fact]
        public void SorterCoursesOrderByDescPrice()
        {
            string sorter = GlobalConstants.OrderDescByPrice;
            var courses = GetCoursesCollection();

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var sortedCourses = mappedCourses.OrderByDescending(x => x.Price);

            var service = new CourseService(null, this.repository.Object, null, null, null, null);
            var result = service.Sorter(sorter, mappedCourses.AsQueryable());

            Assert.Equal(sortedCourses, result);
        }

        [Fact]
        public void SorterCoursesOrderByNewestDate()
        {
            string sorter = GlobalConstants.OrderByNewestDate;
            var courses = GetCoursesCollection();

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var sortedCourses = mappedCourses.OrderByDescending(x => x.CreatedOn);

            var service = new CourseService(null, this.repository.Object, null, null, null, null);
            var result = service.Sorter(sorter, mappedCourses.AsQueryable());

            Assert.Equivalent(sortedCourses, result, strict: true);
        }

        [Fact]
        public void SorterCoursesOrderByOldestDate()
        {
            string sorter = GlobalConstants.OrderByOldestDate;
            var courses = GetCoursesCollection();

            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var sortedCourses = mappedCourses.OrderBy(x => x.CreatedOn);

            var service = new CourseService(null, this.repository.Object, null, null, null, null);
            var result = service.Sorter(sorter, mappedCourses.AsQueryable());

            Assert.Equivalent(sortedCourses, result, strict: true);
        }

        [Theory]
        [InlineData(Difficulty.Beginner)]
        [InlineData(Difficulty.Intermediate)]
        [InlineData(Difficulty.Advanced)]
        public async Task SearchCoursesByDifficulty(Difficulty difficulty)
        {
            var courses = GetCoursesCollection();
            var coursesByDifficulty = courses.Where(x => x.Difficulty == difficulty);

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(courses.BuildMock());
            var mappedCourses = this.Mapper.Map<List<BaseCourseViewModel>>(courses);

            var searchModel = new SearchViewModel()
            {
                Difficulty = (int)difficulty,
                Courses = mappedCourses,
            };

            var service = new CourseService(null, this.repository.Object, null, null, null, null);

            await service.SearchCourses(searchModel);

            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
            Assert.Equal(coursesByDifficulty.Count(), searchModel.Courses.Count());
            Assert.All(searchModel.Courses, x => Assert.Equal(difficulty.ToString(), x.Difficulty));
        }

        public static List<Course> GetCoursesCollection()
        {
            var courses = new List<Course>();
            var user1 = new ApplicationUser() { FirstName = "Peter", Nickname = "peter" };
            var user2 = new ApplicationUser() { FirstName = "Gosho", Nickname = "gosho" };
            var categories = CategoryServiceTests.GetCategoryList();
            var languages = LanguageServiceTests.GetLanguages();

            courses.Add(new Course
            {
                Owner = user2,
                Id = 1,
                Title = "Test1",
                Price = 20,
                MainImageUrl = null,
                IsFree = false,
                CategoryId = 1,
                Category = categories[0],
                Difficulty = Difficulty.Beginner,
                LanguageId = 1,
                Language = languages[0],
                Requirments = "test1",
                Description = "test1",
            });
            courses.Add(new Course
            {
                Owner = new ApplicationUser() { FirstName = "Pesho", Nickname = "pesho" },
                Id = 2,
                Title = "Test2",
                Price = 100,
                MainImageUrl = null,
                IsFree = false,
                CategoryId = 2,
                Category = categories[1],
                Difficulty = Difficulty.Intermediate,
                LanguageId = 2,
                Language = languages[1],
                Requirments = "test2",
                Description = "test2",
            });
            courses.Add(new Course
            {
                Owner = new ApplicationUser() { FirstName = "Iwan", Nickname = "ivan" },
                Id = 3,
                Title = "Test3",
                Price = 150,
                MainImageUrl = null,
                IsFree = false,
                CategoryId = 3,
                Category = categories[2],
                Difficulty = Difficulty.Advanced,
                LanguageId = 3,
                Language = languages[2],
                Requirments = "test3",
                Description = "test3",
            });
            courses.Add(new Course
            {
                Owner = new ApplicationUser() { FirstName = "Marto", Nickname = "marto" },
                Id = 4,
                Title = "Test4",
                Price = 0,
                MainImageUrl = null,
                IsFree = true,
                CategoryId = 4,
                Category = categories[3],
                Difficulty = Difficulty.Advanced,
                LanguageId = 4,
                Language = languages[3],
                Requirments = "test4",
                Description = "test4",
                CourseStudents = new HashSet<StudentCourse>()
                {
                    new StudentCourse { CourseId = 4, UserId = user1.Id, User = user1 },
                    new StudentCourse { CourseId = 4, UserId = user2.Id, User = user2 },
                },
            });
            courses.Add(new Course
            {
                Owner = new ApplicationUser() { FirstName = "Dani", Nickname = "dani" },
                Id = 5,
                Title = "Test5",
                Price = 0,
                MainImageUrl = null,
                IsFree = true,
                CategoryId = 5,
                Category = categories[4],
                Difficulty = Difficulty.Advanced,
                LanguageId = 5,
                Language = languages[4],
                Requirments = "test5",
                Description = "test5",
            });
            courses.Add(new Course
            {
                Owner = user1,
                Id = 6,
                Title = "Test7",
                Price = 0,
                MainImageUrl = null,
                IsFree = true,
                CategoryId = 5,
                Category = categories[4],
                Difficulty = Difficulty.Beginner,
                LanguageId = 5,
                Language = languages[4],
                Requirments = "test6",
                Description = "test6",
            });
            courses.Add(new Course
            {
                Owner = user1,
                Id = 7,
                Title = "Test8",
                Price = 300,
                MainImageUrl = null,
                IsFree = false,
                CategoryId = 5,
                Category = categories[4],
                Difficulty = Difficulty.Advanced,
                LanguageId = 5,
                Language = languages[4],
                Requirments = "test6",
                Description = "test6",
            });
            courses.Add(new Course
            {
                Owner = user1,
                Id = 8,
                Title = "Test9",
                Price = 230,
                MainImageUrl = null,
                IsFree = false,
                CategoryId = 5,
                Category = categories[4],
                Difficulty = Difficulty.Advanced,
                LanguageId = 5,
                Language = languages[4],
                Requirments = "test6",
                Description = "test6",
            });
            courses.Add(new Course
            {
                Owner = user1,
                Id = 9,
                Title = "Test10",
                Price = 135,
                MainImageUrl = null,
                IsFree = false,
                CategoryId = 5,
                Category = categories[4],
                Difficulty = Difficulty.Intermediate,
                LanguageId = 5,
                Language = languages[4],
                Requirments = "test6",
                Description = "test6",
            });
            courses.Add(new Course
            {
                Owner = user2,
                Id = 10,
                Title = "Test11",
                Price = 135,
                MainImageUrl = null,
                IsFree = false,
                CategoryId = 5,
                Category = categories[4],
                Difficulty = Difficulty.Advanced,
                LanguageId = 5,
                Language = languages[4],
                Requirments = "test6",
                Description = "test6",
            });
            courses.Add(new Course
            {
                Owner = user2,
                Id = 11,
                Title = "Test12",
                Price = 56,
                MainImageUrl = null,
                IsFree = false,
                CategoryId = 5,
                Category = categories[4],
                Difficulty = Difficulty.Beginner,
                LanguageId = 5,
                Language = languages[4],
                Requirments = "test6",
                Description = "test6",
            });

            return courses;
        }

        private ImportCourseModel CreateModel()
        {
            var owner = new BaseUserViewModel { Id = "22" };
            return new ImportCourseModel
            {
                Owner = owner,
                Id = 5,
                Title = "Test4",
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
