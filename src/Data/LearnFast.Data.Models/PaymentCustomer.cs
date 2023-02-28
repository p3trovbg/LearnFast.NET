namespace LearnFast.Data.Models
{
    using LearnFast.Data.Common.Models;

    public class PaymentCustomer : BaseDeletableModel<int>
    {
        public string Email { get; set; }

        public string AccountNumber { get; set; }

        public string RoutingNumber { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
