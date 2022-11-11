namespace LearnFast.Services.Data.CountryService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountryService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllOrderByAlphabeticalAsync<T>();
    }
}
