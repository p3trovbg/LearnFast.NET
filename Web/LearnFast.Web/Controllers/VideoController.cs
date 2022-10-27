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
    }
}
