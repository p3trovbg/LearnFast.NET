using LearnFast.Data.Models;
using LearnFast.Services.Mapping;
using LearnFast.Web.ViewModels.ApplicationUser;

namespace LearnFast.Web.ViewModels.Course
{
    public class PaymentCourseModel : IMapFrom<LearnFast.Data.Models.Course>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string MainImageUrl { get; set; }

        public PaymentUserViewModel Owner { get; set; }
    }
}
