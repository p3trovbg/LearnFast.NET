namespace LearnFast.Services.Data.ContactService
{
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Home;

    public interface IContactService
    {
        Task AcceptingMessage(InputContactViewModel model);
    }
}
