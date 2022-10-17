using LearnFast.Web.ViewModels.Language;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnFast.Services.Data
{
    public interface ILanguageService
    {
        Task<LanguageViewModel> GetLanguageByIdAsync(int id);

        IEnumerable<T> GetAllLanguage<T>();
    }
}
