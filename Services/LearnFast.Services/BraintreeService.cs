namespace LearnFast.Services
{
    using Braintree;
    using Microsoft.Extensions.Configuration;

    public class BraintreeService : IBraintreeService
    {
        private string merchantId;
        private string publicId;
        private string privateId;

        public BraintreeService(
            string merchantId,
            string publicId,
            string privateId)
        {
            this.merchantId = merchantId;
            this.publicId = publicId;
            this.privateId = privateId;
        }

        public IBraintreeGateway CreateGateway()
        {
            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = this.merchantId,
                PublicKey = this.publicId,
                PrivateKey = this.privateId,
            };

            return gateway;
        }

        public IBraintreeGateway GetGateway()
        {
            return this.CreateGateway();
        }
    }
}
