namespace LearnFast.Services.Data.LanguageService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LearnFast.Web.ViewModels.Language;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ILanguageService
    {
        Task<LanguageViewModel> GetLanguageByIdAsync(int id);

        Task<IEnumerable<T>> GetAllLanguageAsync<T>();

        Task<IEnumerable<SelectListItem>> GetLanguagesAsSelectListItem();
    }
}
