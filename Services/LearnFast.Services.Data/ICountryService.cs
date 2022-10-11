using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnFast.Services.Data
{
    public interface ICountryService
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllOrderByAlphabetical<T>();
    }
}
