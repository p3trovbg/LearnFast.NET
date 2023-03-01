using LearnFast.Data.Models;
using LearnFast.Services.Mapping;

namespace LearnFast.Web.ViewModels.ApplicationUser
{
    public class PaymentUserViewModel : IMapFrom<LearnFast.Data.Models.ApplicationUser>
    {
        public string Id { get; set; }

        public string StripeId { get; set; }
    }
}
