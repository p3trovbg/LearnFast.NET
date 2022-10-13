using LearnFast.Web.ViewModels.Language;
using System.Threading.Tasks;

namespace LearnFast.Services.Data
{
    public interface ILanguageService
    {
        Task<LanguageViewModel> GetLanguageByIdAsync(int id);
    }
}
