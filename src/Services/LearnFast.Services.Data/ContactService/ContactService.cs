namespace LearnFast.Services.Data.ContactService
{
    using System.Threading.Tasks;

    using LearnFast.Common;
    using LearnFast.Services.Messaging;
    using LearnFast.Web.ViewModels.Home;

    public class ContactService : IContactService
    {
        private const string Subject = "Message";

        private readonly IEmailSender emailSender;

        public ContactService(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public async Task AcceptingMessage(InputContactViewModel model)
        {
            await this.emailSender
                .SendEmailAsync(GlobalConstants.AppEmail, model.Name, GlobalConstants.EmailTaker, Subject + " " + model.Email, model.Message);
        }
    }
}
