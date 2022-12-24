namespace LearnFast.Data.Models
{
    using LearnFast.Data.Common.Models;

    public class PaymentCustomer : BaseDeletableModel<int>
    {
        public string CustomerIdentifier { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
