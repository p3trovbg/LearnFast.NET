namespace LearnFast.Services
{
    using Braintree;

    public interface IBraintreeService
    {
        IBraintreeGateway CreateGateway();

        IBraintreeGateway GetGateway();
    }
}
