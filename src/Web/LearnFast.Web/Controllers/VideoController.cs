namespace LearnFast.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using LearnFast.Services.Data;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Data.VideoService;
    using LearnFast.Web.ViewModels.Content;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class VideoController : BaseController
    {
        private readonly IVideoService videoService;
        private readonly ICourseService courseService;
        private readonly IUserService userService;

        public VideoController(
            IVideoService videoService,
            ICourseService courseService,
            IUserService userService)
        {
            this.videoService = videoService;
            this.courseService = courseService;
            this.userService = userService;
        }

        public async Task<IActionResult> AddVideo(int courseId)
        {
            var currentUserId = await this.userService.GetLoggedUserIdAsync();
            var ownerId = await this.courseService.GetOwnerIdByCourse(courseId);

            if (currentUserId != ownerId)
            {
                return this.Forbid();
            }

            var model = new ImportVideoModel();
            model.CourseId = courseId;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo(ImportVideoModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.View(model);
            }

            try
            {
                await this.videoService.UploadVideo(model);
                return this.RedirectToAction(
                    CourseController.DetailsActionName, CourseController.CourseNameController, new { id = model.CourseId });
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> RemoveVideo(string videoId, int courseId)
        {
            var currentUserId = await this.userService.GetLoggedUserIdAsync();
            var ownerId = await this.courseService.GetOwnerIdByCourse(courseId);

            if (currentUserId != ownerId)
            {
                return this.Forbid();
            }

            try
            {
                await this.videoService.RemoveVideo(videoId);

                return this.RedirectToAction(
                    CourseController.DetailsActionName, CourseController.CourseNameController, new { id = courseId });
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditVideo(EditVideoViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.View(nameof(this.Edit), model);
            }

            var currentUserId = await this.userService.GetLoggedUserIdAsync();
            var ownerId = await this.courseService.GetOwnerIdByCourse(model.CourseId);

            if (currentUserId != ownerId)
            {
                return this.NotFound();
            }

            try
            {
                await this.videoService.EditVideo(model);

                return this.RedirectToAction(
                    CourseController.DetailsActionName, CourseController.CourseNameController, new { id = model.CourseId });
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Edit(EditVideoViewModel model)
        {
            var currentUserId = await this.userService.GetLoggedUserIdAsync();

            var ownerId = await this.courseService.GetOwnerIdByCourse(model.CourseId);

            if (currentUserId != ownerId)
            {
                return this.NotFound();
            }

            return this.View(nameof(this.EditVideo), model);
        }
    }
}
