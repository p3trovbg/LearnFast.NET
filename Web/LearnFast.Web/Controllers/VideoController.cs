namespace LearnFast.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using LearnFast.Services.Data.VideoService;
    using LearnFast.Web.ViewModels.Content;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class VideoController : BaseController
    {
        private readonly IVideoService videoService;

        public VideoController(IVideoService videoService)
        {
            this.videoService = videoService;
        }

        public IActionResult AddVideo(int courseId, string ownerId)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != ownerId)
            {
                return this.NotFound();
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
                this.BadRequest();
            }

            await this.videoService.UploadVideo(model);
            return this.RedirectToAction("Details", "Course", new { id = model.CourseId });
        }

        public async Task<IActionResult> RemoveVideo(string videoId, string ownerId, int courseId)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != ownerId)
            {
                return this.NotFound();
            }

            await this.videoService.RemoveVideo(videoId);

            return this.RedirectToAction("Details", "Course", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> EditVideo(EditVideoViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.BadRequest();
            }

            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId != model.OwnerId)
            {
                return this.NotFound();
            }

            await this.videoService.EditVideo(model);

            return this.RedirectToAction("Details", "Course", new { id = model.CourseId });
        }

        public IActionResult Edit(EditVideoViewModel model)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId != model.OwnerId)
            {
                return this.NotFound();
            }

            return this.View("EditVideo", model);
        }
    }
}
